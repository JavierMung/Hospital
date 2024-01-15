import React, { useState, useEffect } from 'react';
import './Medico.css'
const id = 6;
const ManageAppointment = () => {

  const [citas, setCitas] = useState([]); // Estado para almacenar la lista de citas
  const [selectedCita, setSelectedCita] = useState(null); // Estado para la cita seleccionada

// Estado que refleja la estructura completa del objeto cita para la actualización
const [appointmentToUpdate, setAppointmentToUpdate] = useState({
    id: 0,
    fechaAlta: '',
    fechaCita: '',
    paciente: {
      id: 0,
      nombre: '',
      apellido_Paterno: '',
      apellido_Materno: '',
      edad: 0,
      curp: ''
    },
    idMedico: 0,
    costo: 0,
    idServicio: 0,
    status: ''
  });

  const toLocalDateTime = (isoString) => {
    const date = new Date(isoString);
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0'); // getMonth() es 0-indexado
    const day = date.getDate().toString().padStart(2, '0');
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  };
  // Y luego usarías la función para convertir la fecha antes de asignarla al valor del input:
  const fechaAltaFormatted = selectedCita ? toLocalDateTime(selectedCita.fechaAlta) : '';
  const fechaCitaFormatted = selectedCita ? toLocalDateTime(selectedCita.fechaCita) : '';
  
  // Función para cargar las citas desde la API
  const fetchCitas = async () => {
    try {
      const response = await fetch(`https://localhost:7079/Citas/obtenerCitasByMedicoId/${id}`);
      if (!response.ok) {
        throw new Error('No se pudieron obtener las citas');
      }
      const data = await response.json();
      setCitas(data.model);
    } catch (error) {
      console.error(error);
    }
  };

  // Función para enviar los datos actualizados de la cita a la API
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch('https://localhost:7079/Citas/actualizarCita', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(selectedCita)
      });

      if (!response.ok) {
        throw new Error('No se pudo actualizar la cita');
      }
      console.log('Cita actualizada con éxito');
    } catch (error) {
      console.error(error);
    }
  };

  // useEffect para cargar las citas al montar el componente
  useEffect(() => {
    fetchCitas();
  }, []);

  // Manejador para cuando se selecciona una cita
  const handleSelectChange = (event) => {
    const citaId = event.target.value;
    const cita = citas.find(c => c.id.toString() === citaId);
    setSelectedCita(cita);
  };
  //Manejador cuando cambia el formulario
  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setSelectedCita((prevCita) => ({
      ...prevCita,
      [name]: value
    }));
  };
  

  return (
    <div className="form-container">
      <label htmlFor="citaSelect">Selecciona una cita:</label>
      <select id="citaSelect" onChange={handleSelectChange}>
        <option value="">--Selecciona una cita--</option>
        {citas.map((cita) => (
          <option key={cita.id} value={cita.id}>
            {`Cita ID: ${cita.id}, Paciente: ${cita.paciente.nombre}`}
          </option>
        ))}
      </select>
      {selectedCita && (
        <form onSubmit={handleSubmit} >
          <input className="form-input" type="hidden" name="id" value={selectedCita.id} readOnly/>
          <label>Fecha de Alta:</label>
          <input className="form-input" type="datetime-local" name="fechaAlta" value={fechaAltaFormatted} onChange={handleFormChange} />
          <label>Fecha de la Cita:</label>
          <input className="form-input" type="datetime-local" name="fechaCita" value={selectedCita.fechaCita} onChange={handleFormChange} />
          {/* Agrega campos para cada propiedad que necesites del objeto paciente */}
          <label>ID Paciente:</label>
          <input className="form-input" type="number" name="idMedico" value={selectedCita.paciente.id} onChange={handleFormChange} />
          <label>Nombre:</label>
          <input className="form-input" type="text" name="nombre" value={selectedCita.paciente.nombre} onChange={handleFormChange} />
          <label>Apellido Paterno:</label>
          <input className="form-input" type="text" name="apellido_Paterno" value={selectedCita.paciente.apellido_Paterno} onChange={handleFormChange} />
          <label>Appelido Materno:</label>
          <input className="form-input" type="text" name="apellido_Materno" value={selectedCita.paciente.apellido_Materno} onChange={handleFormChange} />
          <label>Edad:</label>
          <input className="form-input" type="number" name="edad" value={selectedCita.paciente.edad} onChange={handleFormChange} />
          <label>Curp:</label>
          <input className="form-input" type="text" name="curp" value={selectedCita.paciente.curp} onChange={handleFormChange} />

          <label>Costo:</label>
          <input className="form-input" type="number" name="costo" value={selectedCita.costo} onChange={handleFormChange} />
          <label>ID Servicio:</label>
          <input className="form-input" type="number" name="idServicio" value={selectedCita.idServicio} onChange={handleFormChange} />
          <label>Estatus:</label>
          <select className="form-input" name="status" value={selectedCita.status} onChange={handleFormChange}>
            {/* Opciones de estatus */}
            <option value="En espera">En espera</option>
            <option value="Aprobada">Aprobada</option>
            <option value="Cancelada">Cancelada</option>
          </select>
          <button type="submit">Actualizar Cita</button>
        </form>
  )}  
    </div>
  );
};

export default ManageAppointment;
