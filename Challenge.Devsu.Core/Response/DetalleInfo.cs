namespace Challenge.Devsu.Core.Response
{
    public class DetalleInfo
    {
        public string? Codigo { get; set; }
        public string? Detalle { get; set; }
        public int? Estado { get; set; }
        public string? Etapa { get; set; }
        public bool isWorkflowStopped { get; set; }
        public int StoppingReasonId { get; set; }
        public bool Retoma { get; set; }
    }
}
