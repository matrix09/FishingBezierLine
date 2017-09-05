namespace Assets.Scripts.ConsoleController.Command
{
    using System;
    using System.Runtime.CompilerServices;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class CommandLineAttribute : Attribute
    {
        public string Description { get; set; }

        public string Name { get; set; }
    }
}

