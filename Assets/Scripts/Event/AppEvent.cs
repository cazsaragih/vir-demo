namespace VirTest.Event
{
    public class SwitchClick { }

    public class ModeClick { }

    public class ModeChange
    {
        public string modeName;

        public ModeChange(string modeName)
        {
            this.modeName = modeName;
        }
    }
}
