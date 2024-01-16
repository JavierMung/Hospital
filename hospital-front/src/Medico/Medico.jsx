import React, { useState, useEffect } from 'react';
import './Medico.css'
const id = 6;
const ManageAppointment = () => {

  const [citas, setCitas] = useState([]); // Estado para almacenar la lista de citas
  const [selectedCita, setSelectedCita] = useState(null); // Estado para la cita seleccionada
  const [error, setError] = useState('');
  const [isFormVisible, setIsFormVisible] = useState(false);
  const [isPrescriptionFormVisible, setIsPrescriptionFormVisible] = useState(false);
  const [recetasMedicas, setRecetasMedicas] = useState([]);


  const [prescription, setPrescription] = useState({
    idCita: '',
    posologia: ''
  });

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

  const handleModifyClick = () => {
    setIsFormVisible(!isFormVisible); // Cambia la visibilidad del formulario
  };
  const togglePrescriptionForm = () => {
    setIsPrescriptionFormVisible(!isPrescriptionFormVisible);
  };

  const fetchRecetasMedicas = async () => {
    try {
      const recetas = await Promise.all(citas.map(async (cita) => {
        const response = await fetch(`https://localhost:7079/RecetaMedica/obtenerRecetaMedicabyIdCita/${cita.id}`);
        if (!
        response.ok && response.status !== 204) { // 204 No Content, significa que no hay receta para esa cita
        throw new Error('Error al obtener la receta médica');
        }
        const data = await response.json();
        return data; // La API devuelve un objeto que incluye model, message y status
        }));
        const recetasValidas = recetas.filter(receta => receta.model !== null);
        console.log(recetas)
        setRecetasMedicas(recetasValidas);
        setError('Recetas médicas consultadas correcatemente')
    } catch (error) {
      console.error('Error al obtener recetas médicas:', error);
      setError(error.message);
    }
  };
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

  const handlePrescriptionChange = (e) => {
    const { name, value } = e.target;
    setPrescription({
      ...prescription,
      [name]: value
    });
  };

  const handlePrescriptionSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch('https://localhost:7079/RecetaMedica/agregarRecetaMedica', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(prescription)
      });

      if (!response.ok) {
        throw new Error('Error al enviar la receta médica');
      }

      // Lógica después de una receta médica exitosa
      setError('Receta médica creada exitosamente');
      console.log(response)
      setPrescription({ idCita: '', posologia: '' });

    } catch (error) {
      console.error('Error al enviar la receta médica:', error);
      setError(error.message);
    }
  };

  // Función para enviar los datos actualizados de la cita a la API
  const handleSubmit = async (e) => {
        const fechaAlta = new Date(appointmentToUpdate.fechaAlta);
      fechaAlta.setDate(fechaAlta.getDate() + 1);
      const offset = fechaAlta.getTimezoneOffset() * 60000;
      const fechaISOlocal = (new Date(fechaAlta.getTime() - offset)).toISOString().slice(0, -1);
      
      const fechaCita = new Date(appointmentToUpdate.fechaCita);
      fechaCita.setDate(fechaCita.getDate() + 1);
      const offset2 = fechaCita.getTimezoneOffset() * 60000;
      const fechaCitaISOlocal = (new Date(fechaCita.getTime() - offset2)).toISOString().slice(0, -1);

    e.preventDefault();
    const citaToUpdate = {
        ...appointmentToUpdate,
        fechaAlta: fechaISOlocal,
        fechaCita: fechaCitaISOlocal,
        paciente: { ...appointmentToUpdate.paciente },
        idMedico: appointmentToUpdate.medico.idMedico,
        costo: appointmentToUpdate.costo,
        idServicio: appointmentToUpdate.idServicio,
        status: appointmentToUpdate.status
      };
      if (citaToUpdate.medico) {
        delete citaToUpdate.medico;
      }
    console.log(JSON.stringify(citaToUpdate));
    try {
      const response = await fetch('https://localhost:7079/Citas/actualizarCita', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(citaToUpdate)
      });
      if (!response.ok) {
        const errorData = await response.json();
        setError(errorData.message || 'Error al crear la cita.');
      }
      setError('La cita se ha actualizado correctamente.');
    } catch (error) {
      setError(error);
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
    if (cita) {
      setAppointmentToUpdate({
        ...cita,
        fechaAlta: cita.fechaAlta.split('.')[0], // Remove milliseconds
        fechaCita: cita.fechaCita.split('.')[0] // Remove milliseconds
      });
      setPrescription(prevPrescription => ({
        ...prevPrescription,
        idCita: citaId
      }));
    }
  };
  //Manejador cuando cambia el formulario
  const handleFormChange = (e) => {
    const { name, value } = e.target;
    // Manejar los campos anidados del paciente
    if (name.includes('.')) {
      const [parentKey, childKey] = name.split('.');
      setAppointmentToUpdate(prev => ({
        ...prev,
        [parentKey]: {
          ...prev[parentKey],
          [childKey]: value
        }
      }));
    } else {
      setAppointmentToUpdate(prev => ({
        ...prev,
        [name]: value
      }));
    }
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
  <div>
    <button onClick={handleModifyClick}>{isFormVisible ? 'Ocultar' : 'Visualizar'}</button>
  </div>
  {selectedCita && isFormVisible && (
  <form onSubmit={handleSubmit}>
    <input className="form-input" type="hidden" name="id" value={appointmentToUpdate.id} readOnly />
    <label>Fecha de Alta:</label>
    <input className="form-input" type="hidden"  name="fechaAlta" value={appointmentToUpdate.fechaAlta} readOnly/>
    <label>Fecha de la Cita:</label>
    <input className="form-input" type="datetime-local" name="fechaCita" value={appointmentToUpdate.fechaCita}
      onChange={handleFormChange} />
    {/* Agrega campos para cada propiedad que necesites del objeto paciente */}
    <label>ID Paciente:</label>
    <input className="form-input" type="hidden" name="id" value={appointmentToUpdate.paciente.id} readOnly/>
    <label>Nombre:</label>
    <input className="form-input" type="text" name="nombre" value={appointmentToUpdate.paciente.nombre}
      onChange={handleFormChange} />
    <label>Apellido Paterno:</label>
    <input className="form-input" type="text" name="apellido_Paterno"
      value={appointmentToUpdate.paciente.apellido_Paterno} onChange={handleFormChange} />
    <label>Appelido Materno:</label>
    <input className="form-input" type="text" name="apellido_Materno"
      value={appointmentToUpdate.paciente.apellido_Materno} onChange={handleFormChange} />
    <label>Edad:</label>
    <input className="form-input" type="number" name="edad" value={appointmentToUpdate.paciente.edad}
      onChange={handleFormChange} />
    <label>Curp:</label>
    <input className="form-input" type="text" name="curp" value={appointmentToUpdate.paciente.curp}
      onChange={handleFormChange} />

    <label>IdMedico:</label>
    <input className="form-input" type="hidden" name="idMedico" value={appointmentToUpdate.medico.idMedico} readOnly/>
    <label>Costo:</label>
    <input className="form-input" type="hidden" name="costo" value={appointmentToUpdate.costo} readOnly/>
    <label>ID Servicio:</label>
    <input className="form-input" type="hidden" name="idServicio" value={appointmentToUpdate.idServicio} readOnly/>
    <label>Estatus:</label>
    <select className="form-input" name="status" value={appointmentToUpdate.status} onChange={handleFormChange}>
      {/* Opciones de estatus */}
      <option value="En espera">En espera</option>
      <option value="Aprobada">Aprobada</option>
      <option value="Cancelada">Cancelada</option>
    </select>
    <button type="submit">Actualizar Cita</button>
  </form>
  )}
  <div>
    <button onClick={togglePrescriptionForm}>
      {isPrescriptionFormVisible ? 'Ocultar Formulario de Receta' : 'Agregar Receta Médica'}
    </button>

  </div>
  {/* Formulario de receta médica que se muestra solo si isPrescriptionFormVisible es true */}
  {isPrescriptionFormVisible && (
  <form onSubmit={handlePrescriptionSubmit}>
    <div>
      <label htmlFor="idCita">ID de la Cita:</label>
      <input type="hidden" id="idCita" name="idCita" value={prescription.idCita} readOnly />
    </div>
    <div>
      <label htmlFor="posologia">Posología:</label>
      <input type="text" id="posologia" name="posologia" value={prescription.posologia}
        onChange={handlePrescriptionChange} />
    </div>
    <button type="submit">Enviar Receta</button>
  </form>
  )}
  <div>
  <button onClick={fetchRecetasMedicas}>Obtener Recetas Médicas</button>
  </div>
  <table>
    <thead>
      <tr>
        <th>N° Receta</th>
        <th>Fecha</th>
        <th>Nombre paciente</th>
        <th>Nombre Médico</th>
        <th>Posología</th>
      </tr>
    </thead>
    <tbody>
      {recetasMedicas.map((receta) => (
        <tr key={receta.model.idRecetaMedica}>
          <td>{receta.model.idRecetaMedica}</td>
          <td>{receta.model.cita.fechaCita}</td>
          <td>{receta.model.cita.paciente.nombre}</td>
          <td>{receta.model.cita.medico.nombre}</td>
          <td>{receta.model.posologia}</td>
        </tr>
      ))}
    </tbody>
  </table>
  {error && <p className="error-message">{error}</p>}

</div>
);
};

export default ManageAppointment;
