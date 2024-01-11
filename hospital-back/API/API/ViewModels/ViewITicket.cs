namespace API.ViewModels
{
	public record ViewTicketAdd(int IdTrabajador, List<ViewInsumoTicket> Insumos, List<ViewServicioTicketAdd> Servicios);
	public record ViewTicketResponse(ViewTicketAdd Respuesta, double Total, int IdTicket);
    public record ViewTicket(int IdTicket, int IdTrabajador, double Total, List<ViewInsumoResponse> Insumos, List<ViewServicioTicket> Servicios);
}
