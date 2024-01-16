import Ticket from "./Ticket";
import CrearTicket from "./CrearTicket";
import React, { useState,} from "react";
import "./styles.css";


function Recepcionista() {
  const [botonSeleccionado, setBotonSeleccionado] = useState("");

  return (
    <div>
      <h1>Recepcionista</h1>
      <div>
        {botonSeleccionado === "crear" && <CrearTicket />}
        {botonSeleccionado === "mostrar" && <Ticket />}
      </div>
      <div>
        <p><br/></p>
        <button className="button" onClick={() => setBotonSeleccionado("crear") }style={{margin:"10px"}}>Agregar Ticket</button>
        <button className="button" onClick={() => setBotonSeleccionado("mostrar")}>Mostrar Ticket</button>
      </div>
    </div>
  );
}

export default Recepcionista;