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
    idServicio: 0,
    status: ''
  });
  const [error, setError] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setAppointment({
      ...appointment,
      [name]: value
    });
  };

  const mostrarOcultar = () => {
    const elemento = document.querySelector('.RegistrarCita');
    
    if (elemento.style.display === 'none' || elemento.style.display === '') {
      elemento.style.display = 'block'; // Mostrar el elemento si está oculto
    } else {
      elemento.style.display = 'none'; // Ocultar el elemento si está visible
    }
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

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
        const response = await fetch('https://localhost:7079/Citas/crearCita', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(appointment)
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error('Error Data:', errorData); // Imprime la información de error
            setError(errorData.message || 'Error al crear la cita.');
            return; // Detén la ejecución si hay un error
        }

        const responseData = await response.json();
        console.log('Cita creada exitosamente', responseData);
        // Realiza acciones adicionales como limpiar el formulario o actualizar el estado

    } catch (error) {
        console.error('Error al enviar los datos:', error);
        setError(error.message);
    }
};


  return (
    <div className="form-container">
  <h2>Crear Cita</h2>
  <div>
    <button type="button" onClick={mostrarOcultar}>
      Mostrar-Ocultar
    </button>
  </div>
  <form onSubmit={handleSubmit} className='RegistrarCita'>
    {/* Campos para las fechas */}
    <input className="form-input" type="datetime-local" name="fechaAlta" value={appointment.fechaAlta} onChange={handleChange} />
    <input className="form-input" type="datetime-local" name="fechaCita" value={appointment.fechaCita} onChange={handleChange} />

    {/* Campos para los datos del paciente */}
    <input className="form-input" type="text" name="nombre" placeholder="Nombre del paciente" value={appointment.paciente.nombre} onChange={handlePatientChange} />
    <input className="form-input" type="text" name="apellido_Paterno" placeholder="Apellido Paterno" value={appointment.paciente.apellidoPaterno} onChange={handlePatientChange} />
    <input className="form-input" type="text" name="apellido_Materno" placeholder="Apellido Materno" value={appointment.paciente.apellidoMaterno} onChange={handlePatientChange} />
    <input className="form-input" type="number" name="edad" placeholder="Edad"  onChange={handlePatientChange} />
    <input className="form-input" type="text" name="curp" placeholder="CURP"  onChange={handlePatientChange} />
    
    {/* Campos para el médico, costo, servicio y estatus */}
    <input className="form-input" type="number" name="idMedico" placeholder="ID del Médico"  onChange={handleChange} />
    <input className="form-input" type="number" name="costo" placeholder="Costo"  onChange={handleChange} />
    <input className="form-input" type="number" name="idServicio" placeholder="ID del Servicio"  onChange={handleChange} />
    <input className="form-input" type="text" name="status" placeholder="Estado de la cita" value={appointment.status} onChange={handleChange} />

    {/* Botón para enviar el formulario */}
    <button className="form-button" type="submit">Crear Cita</button>
  </form>
  {error && <p className="error-message">{error}</p>}
</div>

    

  );
};

export default CreateAppointment;
