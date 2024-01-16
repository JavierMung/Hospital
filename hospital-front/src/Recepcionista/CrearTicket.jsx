import React, { useState, useEffect } from "react";

const CrearTicket = () => {
  const [insumos, setInsumos] = useState('');
  const [servicios, setServicios] = useState('');
  const [idTrabajador, setIdTrabajador] = useState('');
 
      useEffect(() => {
        const fetchInsumos = async () => {
          const response = await fetch(`https://localhost:7079/Insumos/obtenerInsumos`);
          const data = await response.json();
          setInsumos(data.model);
        };

        const fetchServicios = async () => {
          const response = await fetch(`https://localhost:7079/Servicios/obtenerServicios`);
          const data = await response.json();
          setServicios(data.model);
        }
        fetchInsumos();
        fetchServicios();
      }, []);

  const enviarTicket = () => {
    idTrabajador;
    const insumosFiltrados = insumos.filter(insumo =>insumo.cantidad && insumo.cantidadInsumo !== "");
    const serviciosFiltrados = servicios.filter(servicio =>servicio.cantidad && servicio.cantidadServicio !== "");
    
    const ticket = {
      "idTrabajador":idTrabajador,
    };
    // Envía el ticket a la API Ticket/crearTicket
    ticket.insumos = insumosFiltrados.map(insumo => ({ idInsumo: insumo.idInsumo, cantidad: insumo.cantidad }));
    ticket.servicios = serviciosFiltrados.map(servicio => ({ idServicio: servicio.idServicio, cantidad: servicio.cantidad }));
    fetch('https://localhost:7079/Ticket/crearTicket', {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(ticket),
    })
      .then((response) => response.json())
      .then((data) => {
        if (data.success) {
          alert("Ticket creado correctamente");
        } else {
          alert(data.message);
          window.location.href = "./Recepsionista";
        }
      });
     // document.querySelectorAll("input[name='cantidad']").forEach((input) => input.value = " ");
  };

  return (
    <div>
      <h2>Crear Ticket</h2>
      <p>
        <input type="number"
              placeholder="IdTrabajador"
              value={idTrabajador}
              onChange={() => setIdTrabajador(event.target.value)}/>
      </p>
      <table>
        <thead>
          <tr>
            <th>Insumos</th>
            <th>Servicios</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>
      <table style={{ width: "100%" }}>
        <thead>
          <tr>
            <th>Insumo</th>
            <th>Cantidad</th>
          </tr>
        </thead>
        <tbody>
              {insumos.length > 0 && insumos.map((insumo) => (
                <tr key={insumo.idInsumo}>
                 <td>{insumo.nombre}</td> 
                  <td><input
                        type="number"
                        value={insumo.cantidadInsumo}
                        onChange={(event) => 
                          setInsumos((prevInsumos)=>
                          prevInsumos.map((item)=>
                          item.idInsumo===insumo.idInsumo
                          ?{...item, cantidad: event.target.value}
                          :item)
                )}
                      /></td>
                  </tr>
                ))}
        </tbody>
      </table>
      </td>
      <td>
      <table style={{ width: "100%" }}>
        <thead>
          <tr>
            <th>Servicio</th>
            <th>Cantidad</th>
          </tr>
        </thead>
        <tbody>
                {servicios.length > 0 && servicios.map((servicio) => (
                  <tr key={servicio.idServicio} >
                    <td>{servicio.servicio}</td>
                    <td><input type="number" value={servicio.cantidadServicio} 
                    onChange={(event)=>setServicios((prevServicios)=>
                    prevServicios.map((item)=>
                    item.idServicio===servicio.idServicio
                    ?{...item,cantidad:event.target.value}
                    :item)
                    )}/></td>
                  </tr>
                ))}
        </tbody>
      </table>
      </td>
      </tr>
      </tbody>
      </table>
      <button onClick={enviarTicket}>Crear Ticket</button>
    </div>
  );
};
export default CrearTicket;