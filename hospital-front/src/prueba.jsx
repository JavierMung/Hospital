import React, { useState, useEffect } from 'react';

const WorkerDetails = ({ idTrabajador }) => {
  const [workerData, setWorkerData] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchWorkerData = async () => {
      try {
        const response = await fetch(`https://localhost:7079/Trabajadores/obtenerTrabajador/${idTrabajador}`);
        const data = await response.json();

        if (response.ok) {
          setWorkerData(data.model);
        } else {
          setError(data.message || 'Error al obtener los datos del trabajador');
        }
      } catch (error) {
        console.error('Error al obtener los datos del trabajador:', error);
        setError('Error al conectar con el servidor');
      }
    };

    fetchWorkerData();
  }, [idTrabajador]);

  return (
    <div>
      <h2>Datos del Trabajador</h2>
      {workerData ? (
        <div>
          <p>ID: {workerData.model.idTrabajador}</p>
          <p>Nombre: {workerData.model.persona.nombre}</p>
          <p>Apellido Paterno: {workerData.model.persona.apellido_Paterno}</p>
          <p>Apellido Materno: {workerData.model.persona.apellido_Materno}</p>
          
        </div>
      ) : (
        <p style={{ color: 'red' }}>{error}</p>
      )}
    </div>
  );
};

export default WorkerDetails;
