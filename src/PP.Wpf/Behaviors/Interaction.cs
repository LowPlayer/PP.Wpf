using System.Diagnostics;
using System.Windows;

namespace PP.Wpf.Behaviors
{
    [DebuggerNonUserCode]
    public static class Interaction
    {
        [DebuggerHidden]
        public static Microsoft.Xaml.Behaviors.BehaviorCollection GetBehaviors(DependencyObject obj)
        {
            return Microsoft.Xaml.Behaviors.Interaction.GetBehaviors(obj);
        }

        [DebuggerHidden]
        public static Microsoft.Xaml.Behaviors.TriggerCollection GetTriggers(DependencyObject obj)
        {
            return Microsoft.Xaml.Behaviors.Interaction.GetTriggers(obj);
        }
    }
}
