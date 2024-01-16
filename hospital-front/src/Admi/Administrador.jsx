import React, { useState, useEffect } from 'react';

const Administrador = () => {
  const [medicos, setMedicos] = useState([]);
  const [filtroStatus, setFiltroStatus] = useState('todos'); // Puede ser 'todos', 'activos' o 'inactivos'

  useEffect(() => {
    // Obtener todos los médicos al cargar el componente
    obtenerMedicos();
  }, []);

  const obtenerMedicos = async () => {
    try {
      const response = await fetch('https://localhost:7079/Medicos/obtenerMedicos');
      const data = await response.json();

      if (response.ok) {
        setMedicos(data.model.medicos);
      } else {
        console.error('Error al obtener médicos:', data.message);
      }
    } catch (error) {
      console.error('Error al obtener médicos:', error);
    }
  };

  const verificarCitasAsignadas = async (idMedico) => {
    try {
      const response = await fetch(`https://localhost:7079/Citas/obtenerCitasByMedicoId/${idMedico}`);
      const data = await response.json();

      return response.ok && data.status === 200;
    } catch (error) {
      console.error('Error al verificar citas asignadas:', error);
      return false;
    }
  };

  const actualizarStatusMedico = async (idMedico, nuevoStatus) => {
    try {
      const medicoActual = medicos.find((medico) => medico.idMedico === idMedico);

      if (!medicoActual) {
        console.error('No se encontró el médico con el ID:', idMedico);
        return;
      }

      // Verificar si el médico tiene citas asignadas
      const tieneCitasAsignadas = await verificarCitasAsignadas(idMedico);

      if (tieneCitasAsignadas) {
        alert('No se puede eliminar al médico ya que tiene citas asignadas.');
        return;
      }

      const response = await fetch('https://localhost:7079/Medicos/actualizarMedico', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          idMedico,
          nombre: medicoActual.nombre,
          especialidad: medicoActual.especialidad,
          consultorio: medicoActual.consultorio,
          status: nuevoStatus,
        }),
      });

      if (response.ok) {
        // Actualizar la lista de médicos después de cambiar el estado
        obtenerMedicos();
      } else {
        const data = await response.json();
        console.error('Error al actualizar el estado del médico:', data.errors);
      }
    } catch (error) {
      console.error('Error al actualizar el estado del médico:', error);
    }
  };

  const filtrarMedicos = () => {
    if (filtroStatus === 'todos') {
      return medicos;
    } else {
      return medicos.filter((medico) => medico.status === filtroStatus.toUpperCase());
    }
  };

  return (
    <div>
      <h2>Eliminar </h2>
      <div>
        <label>Filtrar por estado:</label>
        <select value={filtroStatus} onChange={(e) => setFiltroStatus(e.target.value)}>
          <option value="todos">Todos</option>
          <option value="activos">Activos</option>
          <option value="inactivos">Inactivos</option>
        </select>
      </div>
      <div>
        <h3>Listado de Médicos</h3>
        <ul>
          {filtrarMedicos().map((medico) => (
            <li key={medico.idMedico}>
              id: {medico.idMedico} {medico.nombre} - {medico.status}
              <button
                onClick={() =>
                  actualizarStatusMedico(medico.idMedico, medico.status === 'ACTIVO' ? 'INACTIVO' : 'ACTIVO')
                }
              >
                Eliminar
              </button>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default Administrador;
