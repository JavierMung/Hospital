  import React, { useState } from 'react';
  import './Cliente.css'

  const CreateAppointment = () => {
    const [appointment, setAppointment] = useState({
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
      idServicio: 1,
      status: ''
    });
    const [appointments, setAppointments] = useState([]);
    const [error, setError] = useState('');
    const [formVisible, setFormVisible] = useState(false); 
    const [idCita, setIdCita] = useState(''); // Estado para almacenar el idCita del segundo formulario
    const [idCitaFormVisible, setIdCitaFormVisible] = useState(false); // Estado para controlar la visibilidad del formulario de obtención de cita por ID

    const toggleForm = () => {
      setFormVisible(!formVisible);
    };

    const toggleIdCitaForm = () => {
      setIdCitaFormVisible(!idCitaFormVisible);
    };

    const handleChange = (e) => {
      const { name, value } = e.target;
      setAppointment({
        ...appointment,
        [name]: value
      });
    };

    const handlePatientChange = (e) => {
      const { name, value } = e.target;
      setAppointment({
        ...appointment,
        paciente: {
          ...appointment.paciente,
          [name]: value
        }
      });
    };

    const getLocalIsoDate = () => {
      const now = new Date();
      const timezoneOffset = now.getTimezoneOffset() * 60000; // Convertir la diferencia de zona horaria a milisegundos
      const localIsoDate = new Date(now - timezoneOffset).toISOString().slice(0, -1); // Quitar la 'Z' del final
      return localIsoDate;
    };

    const handleSubmit = async (e) => {

      // const fechaAlta = new Date(appointment.fechaAlta);
      // fechaAlta.setDate(fechaAlta.getDate() + 1);
      // const offset = fechaAlta.getTimezoneOffset() * 60000;
      // const fechaISOlocal = (new Date(fechaAlta.getTime() - offset)).toISOString().slice(0, -1);
      
      const fechaCita = new Date(appointment.fechaCita);
      fechaCita.setDate(fechaCita.getDate() + 1);
      const offset2 = fechaCita.getTimezoneOffset() * 60000;
      const fechaCitaISOlocal = (new Date(fechaCita.getTime() - offset2)).toISOString().slice(0, -1);



      let appointmentData = {...appointment};
      appointmentData.fechaAlta = getLocalIsoDate();
      appointmentData.fechaCita = fechaCitaISOlocal;
      e.preventDefault();
      try {
        const response = await fetch('https://localhost:7079/Citas/crearCita', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(appointmentData)
        });

        if (!response.ok) {
          const errorData = await response.json();
          console.error('Error Data:', errorData);
          setError(errorData.message || 'Error al crear la cita.');
          return;
        }

        const responseData = await response.json();
        console.log('Cita creada exitosamente', responseData);
        // Realiza acciones adicionales como limpiar el formulario o actualizar el estado

      } catch (error) {
        console.error('Error al enviar los datos:', error);
        setError(error.message);
      }
    };

    const handleIdCitaChange = (e) => {
      setIdCita(e.target.value);
    };

    const handleIdCitaSubmit = async (e) => {
      e.preventDefault();
      try {
        const response = await fetch(`https://localhost:7079/Citas/obtenerCitasByCURP?CURP=${idCita}`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json'
          }
        });

        if (!response.ok) {
          const errorData = await response.json();
          console.error('Error Data:', errorData);
          setError(errorData.message || 'Error al obtener las citas.');
          return;
        }

        const responseData = await response.json();
        // console.log('Citas obtenidas exitosamente', responseData);
        setAppointments(responseData.model); // Guardamos las citas en el estado

      } catch (error) {
        console.error('Error al obtener las citas:', error);
        setError(error.message);
      }
    };
    
    const cancelarCita = async (cita) => {

      const fechaAlta = new Date(cita.fechaAlta);
      fechaAlta.setDate(fechaAlta.getDate() + 1);
      const offset = fechaAlta.getTimezoneOffset() * 60000;
      const fechaISOlocal = (new Date(fechaAlta.getTime() - offset)).toISOString().slice(0, -1);
      
      const fechaCita = new Date(cita.fechaCita);
      fechaCita.setDate(fechaCita.getDate() + 1);
      const offset2 = fechaCita.getTimezoneOffset() * 60000;
      const fechaCitaISOlocal = (new Date(fechaCita.getTime() - offset2)).toISOString().slice(0, -1);
            
      const citaParaCancelar = {
        id: cita.id,
        fechaAlta: fechaISOlocal,
        fechaCita: fechaCitaISOlocal,
        paciente: {
          id: cita.paciente.id,
          nombre: cita.paciente.nombre,
          apellido_Paterno: cita.paciente.apellido_Paterno,
          apellido_Materno: cita.paciente.apellido_Materno,
          edad: cita.paciente.edad,
          curp: cita.paciente.curp
        },
        idMedico: cita.medico.idMedico,
        costo: cita.costo,
        idServicio: cita.idServicio,
        status: "Cancelada"
      };
      console.log(citaParaCancelar)
      try {
        const response = await fetch('https://localhost:7079/Citas/actualizarCita', {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(citaParaCancelar)
        });
    
        if (!response.ok) {
          const errorData = await response.json();
          console.error('Error al cancelar la cita:', errorData);
          setError(errorData.message || 'Error al cancelar la cita.');
          return;
        }else{
          console.log('Cita actualizada con éxito');
        }
      } catch (error) {
        console.error('Error al enviar la solicitud de cancelación:', error);
        setError(error.message);
      }
    };

    return (
      <div className="form-container">
        <h2>Crear Cita</h2>
        <button onClick={toggleForm}>
          {formVisible ? 'Cancelar' : 'Crear cita'}
        </button>
        {formVisible && (
          <form onSubmit={handleSubmit} className='RegistrarCita'>
          {/* Campos para las fechas */}
          {/* <input className="form-input" type="datetime-local" type="hidden" readOnly name="fechaAlta" value={appointment.fechaAlta} onChange={handleChange} /> */}
          <input className="form-input" type="datetime-local" name="fechaCita" value={appointment.fechaCita} onChange={handleChange} />
      
          {/* Campos para los datos del paciente */}
          <input className="form-input" type="text" name="nombre" placeholder="Nombre del paciente" value={appointment.paciente.nombre} onChange={handlePatientChange} />
          <input className="form-input" type="text" name="apellido_Paterno" placeholder="Apellido Paterno" value={appointment.paciente.apellidoPaterno} onChange={handlePatientChange} />
          <input className="form-input" type="text" name="apellido_Materno" placeholder="Apellido Materno" value={appointment.paciente.apellidoMaterno} onChange={handlePatientChange} />
          <input className="form-input" type="number" name="edad" placeholder="Edad"  onChange={handlePatientChange} />
          <input className="form-input" type="text" name="curp" placeholder="CURP"  onChange={handlePatientChange} />
          
          {/* Campos para el médico, costo, servicio y estatus */}
          <input className="form-input" type="number" name="idMedico" placeholder="ID del Médico"  onChange={handleChange} />
          {/* <input className="form-input" type="number" name="costo" placeholder="Costo"  onChange={handleChange} /> */}
          {/* <input className="form-input" type="number" name="idServicio" placeholder="ID del Servicio"  onChange={handleChange} /> */}
          {/* <input className="form-input" type="text" name="status" placeholder="Estado de la cita" value={appointment.status} onChange={handleChange} /> */}
          
          {/* Botón para enviar el formulario */}
          <button className="form-button" type="submit">Crear Cita</button>
        </form>
        )}

        {/* Segundo formulario para obtener cita por idCita */}
        <h2>Obtener Cita por ID</h2>
        <button onClick={toggleIdCitaForm}>
          {idCitaFormVisible ? 'Cancelar' : 'Mostrar mis citas'}
        </button>
        {idCitaFormVisible && (
          <form onSubmit={handleIdCitaSubmit}>
            <input
              className="form-input"
              type="text"
              name="idCita"
              placeholder="ID de la Cita"
              value={idCita}
              onChange={handleIdCitaChange}
            />
            <button className="form-button" type="submit">Obtener Cita</button>
            <h3>Citas Obtenidas</h3>
            <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Fecha de Alta</th>
                <th>Fecha de la Cita</th>
                <th>Paciente</th>
                <th>Médico</th>
                <th>Costo</th>
                <th>ID del Servicio</th>
                <th>Estatus</th>
              </tr>
            </thead>
            <tbody>
              {appointments.map((appointment) => {
                const fechaAltaDate = new Date(appointment.fechaAlta);
                const fechaCitaDate = new Date(appointment.fechaCita);              
                const fechaAltaFormatted = fechaAltaDate.toLocaleString('es-MX', { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' });
                const fechaCitaFormatted = fechaCitaDate.toLocaleString('es-MX', { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' });
                return (
                  <tr key={appointment.id}>
                    <td>{appointment.id}</td>
                    <td>{fechaAltaFormatted}</td>
                    <td>{fechaCitaFormatted}</td>
                    <td>{`${appointment.paciente.nombre} ${appointment.paciente.apellido_Paterno} ${appointment.paciente.apellido_Materno}`}</td>
                    <td>{`${appointment.medico.nombre} (${appointment.medico.especialidad})`}</td>
                    <td>${appointment.costo}</td>
                    <td>{appointment.idServicio}</td>
                    <td>{appointment.status}</td>
                    <td>
                      <button onClick={() => cancelarCita(appointment)}>Cancelar</button>
                    </td>
                  </tr>
                );
              })}
            </tbody>
            </table>
          </form>
          
        )}

        {error && <p className="error-message">{error}</p>}
      </div>
    );

    
  };

  export default CreateAppointment;
