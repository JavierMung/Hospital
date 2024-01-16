import React, { useState, useEffect } from 'react';
import "./styles.css";

const Ticket = () => {
  const [idTicket, setIdTicket] = useState('');
  const [ticketData, setTicketData] = useState(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false); 

  const handleButtonClick = () => {
    fetchTicketData();
  };

  const getInsumos = (ticketData) => {
    return ticketData.insumos;
  };

  const getServicios = (ticketData) => {
    return ticketData.servicios;
  };

  const fetchTicketData = async () => {
    setLoading(true); 
    try {
      const response = await fetch(`https://localhost:7079/Ticket/obtenerTicket/${idTicket}`);
      const data = await response.json();
    
      if (response.ok) {
        setTicketData(data.model);
      } else {
        setError(data.message || 'Error al obtener los datos del ticket');
        setTicketData(null);
      }
    } catch (error) {
      console.error('Error al obtener los datos del ticket:', error);
      setError('Error al conectar con el servidor');
    } finally {
      setLoading(false); 
    }
  };

  useEffect(() => {
    return () => {
        const handleButtonClick = () => {
            fetchTicketData();
          };
      if (fetchTicketData.cancel) {
        fetchTicketData.cancel();
      }
    };
  }, [idTicket]);

  return (
    <div>
      <p><br/></p>
      <input
        type="text"
        placeholder="ID Ticket"
        onChange={(event) => setIdTicket(event.target.value)}
      />
      <button onClick={handleButtonClick}>Obtener Ticket</button>

      {loading && <p>Cargando...</p>}

      {!loading && ticketData ? (
        <div>
          <p><br/></p>
          <table className='Table'>
            <thead>
              <th>Ticket</th>
              <th>Insumos</th>
              <th>Servicios</th>
            </thead>

           <tbody>
            <tr>
              <td>
                <p>ID: {ticketData.idTicket}</p>
                <p>ID Trabajador: {ticketData.idTrabajador}</p>
                <b>Total: ${ticketData.total}</b>
              </td>
              <td>
          {getInsumos(ticketData).map((insumo) => (
            <div key={insumo.idInsumo}>
              
                <p>ID: {insumo.idInsumo}</p>
                <p>Nombre: {insumo.nombre}</p>
                <p>Costo: {insumo.costo}</p>
                <p>Cantidad: {insumo.cantidad}</p>
                <p>Precio total: {insumo.preTotal}</p>
            </div>
          ))}
           </td>
            <td>
          {getServicios(ticketData).map((servicio) => (
            <div key={servicio.idServicio}>
                <p>ID: {servicio.idServicio}</p>
                <p>Servicio: {servicio.servicio}</p>
                <p>Costo: {servicio.costo}</p>
                <p>Cantidad: {servicio.cantidad}</p>
                <p>Precio total: {servicio.preTotal}</p>
            </div>
          ))}
          </td>
          </tr>
          </tbody> 
          </table>
        </div>
      ) : (
        <p style={{ color: 'red' }}>{error}</p>
      )}
    </div>
  );
};

export default Ticket;







