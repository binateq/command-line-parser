namespace Binateq.CommandLine.Fakes
{
    public class StubACommand : _BaseCommand
    {
        public bool Boolean { get; set; }

        public int Int32 { get; set; }

        public double Double { get; set; }

        public string String { get; set; }

        public StubEnum Enum { get; set; }

        public string[] StringArray { get; set; }
    }
}
