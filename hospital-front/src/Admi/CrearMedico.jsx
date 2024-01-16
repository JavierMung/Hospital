import React, { useState } from 'react';

const CrearMedico = () => {
  const [idTrabajador, setIdTrabajador] = useState(0);
  const [consultorio, setConsultorio] = useState('');
  const [especialidad, setEspecialidad] = useState('');
  const [cedula, setCedula] = useState('');
  const [status, setStatus] = useState('ACTIVO');

  const handleCrearMedico = async () => {
    try {
      // Verificar si el médico ya existe antes de enviar la solicitud de creación
      const existeMedico = await verificarExistenciaMedico(idTrabajador);

      if (existeMedico) {
        alert('Ya existe un médico relacionado a ese trabajador.');
        return;
      }

      const response = await fetch('https://localhost:7079/Medicos/agregarMedico', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          idTrabajador,
          consultorio,
          especialidad,
          consulta: true,
          cedula,
          status,
        }),
      });

      if (response.ok) {
        alert('Creación del médico exitosa.');
        // Limpiar los campos después de una creación exitosa
        setIdTrabajador(0);
        setConsultorio('');
        setEspecialidad('');
        setCedula('');
        setStatus('ACTIVO');
      } else {
        const data = await response.json();
        console.error('Error al crear el médico:', data.message);
      }
    } catch (error) {
      console.error('Error al crear el médico:', error);
    }
  };

  const verificarExistenciaMedico = async (idTrabajador) => {
    try {
      const response = await fetch(`https://localhost:7079/Medicos/obtenerMedicoPorTrabajador/${idTrabajador}`);
      const data = await response.json();

      return response.ok && data.status === 200 && data.model !== null;
    } catch (error) {
        alert('Error al verificar la existencia del médico');
        console.error('Error al verificar la existencia del médico:', error);
      return false;
    }
  };

  return (
    <div>
      <h2>Crear Nuevo Médico</h2>
      <div>
        <label>ID Trabajador:</label>
        <input type="number" value={idTrabajador} onChange={(e) => setIdTrabajador(e.target.value)} />
      </div>
      <div>
        <label>Consultorio:</label>
        <input type="text" value={consultorio} onChange={(e) => setConsultorio(e.target.value)} />
      </div>
      <div>
        <label>Especialidad:</label>
        <input type="text" value={especialidad} onChange={(e) => setEspecialidad(e.target.value)} />
      </div>
      <div>
        <label>Cédula:</label>
        <input type="text" value={cedula} onChange={(e) => setCedula(e.target.value)} />
      </div>
      <div>
        <label>Status:</label>
        <select value={status} onChange={(e) => setStatus(e.target.value)}>
          <option value="ACTIVO">Activo</option>
          <option value="INACTIVO">Inactivo</option>
        </select>
      </div>
      <button onClick={handleCrearMedico}>Crear Médico</button>
    </div>
  );
};

export default CrearMedico;
