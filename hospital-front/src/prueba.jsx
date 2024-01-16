import React, { useState, useEffect, useContext } from 'react';
import TrabajadorId from './App.jsx'

const WorkerDetails = ({ idTrabajador }) => {
  const [workerData, setWorkerData] = useState(null);
  const [error, setError] = useState('');
  const [load, setLoad] = useState(false);
  const [id, setId] = useState(0);
  
  //useEffect(() => {
    
  //}, [])
  const value = useContext(TrabajadorId);
  useEffect(() => {
    
    console.log (idTrabajador);
    const fetchWorkerData = async () => {
      try {
        const response = await fetch(`https://localhost:7079/Trabajadores/obtenerTrabajador/${idTrabajador}`);
        const data = await response.json();

        if (response.ok) {
          setLoad (true);
          setWorkerData(data.model);
          setLoad (true);
        } else {
          setError(data.message || '1');
        }
      } catch (error) {
        console.error('Error al obtener los datos del trabajador:', error);
        setError('2');
      }
    };

    fetchWorkerData();
  }, [idTrabajador]);

  return (
    <div>
      <h2>Datos del Trabajador</h2>
      {load ? (
        
        <div>
          <p>ID: {workerData.idTrabajador}</p>
          <p>Nombre: {workerData.persona.nombre}</p>
          <p>Apellido Paterno: {workerData.persona.apellido_Paterno}</p>
          <p>Apellido Materno: {workerData.persona.apellido_Materno}</p>
          
        </div>
      ) : (
        <p style={{ color: 'red' }}>{error}</p>
      )}

    </div>
  );
};

export default WorkerDetails;
