using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.Helpers;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.DataStore.Editor {
    public class DataStoreEditor {

        [MenuItem("Custom/Open DataStore Exporter Window")]
        public static void CreateDataStoreExporterEditorWindow() {
            EditorWindow.GetWindow<DataStoreEditorWindow>(false, "DataStore Exporter", true);
        }
    }

    public class DataStoreEditorWindow : EditorWindow {

        private class DataStoreEditorWindowState {
            public Vector2 scrollPosition = Vector2.zero;

            public List<string> dataFileDirectories = new List<string>();
            public List<string> sheets = new List<string>();
            public string sheetTag = "";
            public string dbPath;

            public bool verbose;
            public bool debug;
        }

        private DataStoreEditorWindowState state;
        private DataStoreExporter.DataStoreExporterState dsState;

        //OnFocus Called when the window gets keyboard focus.
        public void OnFocus() {
            //this.Log("", "DataStoreEditorWindow OnFocus");

            state = EditorPrefsHelper.GetClass(new DataStoreEditorWindowState());

            //this.LogFormat("", "Time: {0}, {1}", Time.time, DateTime.Now.ToString("HH:mm:ss.ffff"));
        }
        public void LostFocus() {
            //this.Log("", "DataStoreEditorWindow LostFocus");

            EditorPrefsHelper.SetClass(state);
        }

        //OnGUI   Implement your own editor GUI here.
        public void OnGUI() {
            EditorGUIUtility.labelWidth = 80f;

            GUILayout.Space(12f);
            GUILayout.Label("Game DataStore Database Exporter. version:1.0");
            GUILayout.Space(12f);

            if ( GUILayout.Button("Set export database file path.", GUILayout.Width(200)) ) {
                state.dbPath = EditorUtility.SaveFilePanel("Export Game DataStore Database File", "", "GameDataStore", "db");
                //Debug.Log(dbPath);
                EditorPrefsHelper.SetClass(state);
            }
            GUILayout.Label(state.dbPath);
            GUILayout.Space(12f);

            if ( GUILayout.Button("Add excel data file directory.", GUILayout.Width(200)) ) {
                var path = EditorUtility.OpenFolderPanel("Load Excel Data Files of Directory", "", "");
                //Debug.Log(path);
                if ( path.Length > 0 ) {
                    state.dataFileDirectories.Add(path);
                }
                EditorPrefsHelper.SetClass(state);
            }

            foreach ( var path in state.dataFileDirectories.ToArray() ) {
                GUILayout.BeginHorizontal();

                if ( GUILayout.Button("Remove", GUILayout.Width(60)) ) {
                    state.dataFileDirectories.Remove(path);
                    EditorPrefsHelper.SetClass(state);
                }

                GUILayout.Label(path);

                GUILayout.EndHorizontal();
            }
            GUILayout.Space(12f);

            // 
            // Process export
            // 
            GUILayout.BeginHorizontal();

            if ( state.sheets.Count == 0 ) {
                state.sheets.Add("数据");
                state.sheets.Add("数据表");
                state.sheets.Add("数据用表");
            }

            state.sheetTag = GUILayout.TextField(state.sheetTag, 50, GUILayout.Width(200));
            if ( GUILayout.Button("Add excel data sheet tag", GUILayout.Width(200)) ) {
                if ( state.sheets.IndexOf(state.sheetTag) == -1 ) {
                    state.sheets.Add(state.sheetTag);
                    EditorPrefsHelper.SetClass(state);
                }
            }
            GUILayout.EndHorizontal();

            foreach ( var sheet in state.sheets.ToArray() ) {
                GUILayout.BeginHorizontal();

                if ( GUILayout.Button("Remove", GUILayout.Width(60)) ) {
                    state.sheets.Remove(sheet);
                    EditorPrefsHelper.SetClass(state);
                }

                GUILayout.Label(sheet);

                GUILayout.EndHorizontal();
            }
            GUILayout.Space(12f);

            // 
            // Process export
            // 
            //GUILayout.Toggle(false, "Generate Template");
//             if ( GUILayout.Button("Generate Template", GUILayout.Width(200)) ) {
//                 dsState = DataStoreExporter.Prepare(state.sheets, state.dataFileDirectories, state.dbPath, false);
//                 if ( dsState.dbConn != null ) {
// 
//                     DataStoreExporter.GenerateTemplateFile(ref dsState, Application.dataPath + @"\Scripts\DataStore\DataStoreTemplate.cs");
// 
//                     DataStoreExporter.Done(ref dsState);
//                 }
//             }

            GUILayout.BeginHorizontal();
            state.verbose = GUILayout.Toggle(state.verbose, "Verbose", GUILayout.Width(65));
            state.debug = GUILayout.Toggle(state.debug, "Debug", GUILayout.Width(65));
            GUILayout.EndHorizontal();
            if ( GUILayout.Button("Process", GUILayout.Width(200)) ) {

                //DebugHelper.EnableModule("DataStoreExporter");

                EditorUtility.DisplayProgressBar("Export Excel Data Files", "Prepare...", 0);

                dsState = DataStoreExporter.Prepare(state.sheets, state.dataFileDirectories, state.dbPath, true, Application.dataPath + @"\Scripts\DataStore\DataStoreTemplate.cs");
                dsState.verbosity = state.verbose ? 1 : 0;

                if ( dsState.dbConn != null ) {

                    var lastTime = DateTime.Now.Second;
                    while ( !dsState.StartWorking() ) {
                        // Update progress bar
                        if ( lastTime == DateTime.Now.Second ) {
                            continue;
                        }
                        lastTime = DateTime.Now.Second;
                        var progressBar = dsState.donePaths.Count / (float)dsState.excelFilePaths.Count;
                        var percentage = (int)( progressBar * 100f );
                        var progressStr = string.Format("{0:0.00}s {1}% Processing "
                            , ( dsState.stopwatch.ElapsedTicks / (decimal)Stopwatch.Frequency ), percentage);
                        if ( dsState.donePaths.Count > 0 ) {
                            progressStr += dsState.donePaths[dsState.donePaths.Count - 1];
                        }
                        EditorUtility.DisplayProgressBar("Export Excel Data Files", progressStr, progressBar);
                    }

                    DataStoreExporter.AddVersionNum(Application.streamingAssetsPath + "/DBConfig.ini");
                    DataStoreExporter.Done(ref dsState);
                }

                // Remove the progress bar to show that work has finished
                EditorUtility.ClearProgressBar();
            }
            GUILayout.Space(12f);


            state.scrollPosition = GUILayout.BeginScrollView(state.scrollPosition);
            GUILayout.BeginVertical();

            foreach ( var path in state.dataFileDirectories ) {
                var files = DataStoreExporter.GetFiles(path);
                GUILayout.Label(string.Format("{0} Files in directory {1}", files.Length, path));
                foreach ( var file in files ) {
                    GUILayout.BeginHorizontal();

                    if ( dsState != null ) {

                        DrawFileLogButton(file, dsState.doneLogs, "Log", Debug.Log);
                        DrawFileLogButton(file, dsState.doneWarnnings, "Warn", Debug.LogWarning);
                        DrawFileLogButton(file, dsState.doneErrors, "Error", Debug.LogError);

                        var count = dsState.doneRecords.ContainsKey(file)
                            ? dsState.doneRecords[file]
                            : 0;

                        GUILayout.Label(count.ToString(), GUILayout.Width(60));
                    } else {
                        GUILayout.Space(30f);
                    }

                    GUILayout.Label(string.Format("{0}", file));
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.Space(12f);

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void DrawFileLogButton( string file, Dictionary<string, List<string>> doneMsgs, string tag, Action<string> printMsg ) {
            var count = 0;

            if ( doneMsgs.ContainsKey(file) ) {
                var msgs = doneMsgs[file];
                count = msgs.Count;
            }

            if ( count == 0 ) {
                GUILayout.Space(60f);
            } else {
                var msgButton = string.Format("{0}:{1}", tag, count);

                if ( GUILayout.Button(msgButton, GUILayout.Width(60)) ) {
                    if ( doneMsgs.ContainsKey(file) ) {
                        foreach ( var msg in doneMsgs[file] ) {
                            //this.ErrorFormat("", msg);
                            printMsg(msg);
                        }
                    }
                }
            }
        }
    }
}
