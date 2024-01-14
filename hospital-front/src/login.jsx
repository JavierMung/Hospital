import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
//import './index.css'; 

const Login = ({ onLogin }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = () => {
    // Validar el usuario y contraseña
    if (username === 'paco' && password === 'cliente') {
      onLogin(2);
      alert('Inicio de sesion Exitoso');
      navigate('/Recepcionista/Recepsionista');
      
    } else if (username === 'vanesa' && password === 'medico') {
      onLogin(3);
      alert('Inicio de sesion Exitoso');
      navigate('/Recepcionista/Recepsionista');

    } else if (username === 'cancerbero' && password === 'recepcionista') {
      onLogin(4);
      alert('Inicio de sesion Exitoso');
      navigate('/Recepcionista/Recepsionista');
      
    } else {
      alert('Credenciales incorrectas. Por favor, verifica usuario y contraseña.');
    }
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      <div className="input-container">
        <label htmlFor="username">Usuario:</label>
        <input
          type="text"
          id="username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
      </div>
      <div className="input-container">
        <label htmlFor="password">Contraseña:</label>
        <input
          type="password"
          id="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </div>
      <button className="login-button" onClick={handleLogin}>
        Enviar
      </button>
    </div>
  );
};

export default Login;
