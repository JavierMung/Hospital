namespace API.ViewModels
{
	public record ViewTicketAdd(int IdTrabajador, List<ViewInsumoTicket> Insumos, List<ViewServicioTicket> Servicios);
	public record ViewTicketResponse(ViewTicketAdd Respuesta, double Total, int IdTicket);
    public record ViewTicket(int IdTrabajador, double Total, List<ViewInsumoResponse> Insumos, List<ViewServicio> Servicios);
}
