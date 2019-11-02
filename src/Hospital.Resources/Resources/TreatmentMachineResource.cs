namespace Hospital.Resources
{
    public class TreatmentMachineResource
    {
        public string Name { get; set; }
        public string Capability { get; set; }

        public TreatmentMachineResource(string name, string capability)
        {
            Name = name;
            Capability = capability;
        }
    }
}