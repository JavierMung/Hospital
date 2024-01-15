import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const Login = ({ onLogin }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = async () => {
    // Validar el usuario y contraseña
    const credentials = { username, password };

    try {
      const loginResponse = await fetch('https://localhost:7079/User/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(credentials),
      });

      if (!loginResponse.ok) {
        throw new Error('Credenciales incorrectas.');
      }

      const loginData = await loginResponse.json();
      const { idTrabajador, username: returnedUsername } = loginData.model;

      // Obtener datos del trabajador usando el idTrabajador
      const workerResponse = await fetch(`https://localhost:7079/Trabajadores/obtenerTrabajador/${idTrabajador}`);
      const workerData = await workerResponse.json();

      if (workerResponse.ok) {
        const { idRol } = workerData.model;

        // Llamar a onLogin con un objeto que contiene el idRol, username e idTrabajador
        onLogin({ idRol, username: returnedUsername, idTrabajador });

        alert('Inicio de sesión exitoso');
        navigate('/Inicio');
      } else {
        throw new Error('Error al obtener datos del trabajador.');
      }
    } catch (error) {
      console.error('Error de autenticación:', error);
      alert('Credenciales incorrectas. Por favor, verifica usuario y contraseña.');
    }
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      <div className="input-container">
        <label htmlFor="username" className='input'>Usuario: </label>
        <input
          type="text"
          id="username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
      </div>
      <div className="input-container">
        <label htmlFor="password" className='input'>Contraseña: </label>
        <input
          type="password"
          id="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </div>
      <button className="button" onClick={handleLogin}>
        Enviar
      </button>
    </div>
  );
};

export default Login;

