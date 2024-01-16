import React from 'react';
import { useNavigate } from 'react-router-dom';

const Logout = ({ onLogout }) => {
  const navigate = useNavigate();

  const handleLogout = () => {
    onLogout(0); // Función para manejar el cierre de sesión, podría ser similar a handleLogin
    navigate('/login'); // Redirige a la página de inicio de sesión después de cerrar sesión
  };

  return (
    <div>
      <h2 className='login-container'>Estas seguro que deseas cerrar sesion?
      <button onClick={handleLogout} className='button'>Confirmar</button>
      </h2>
    </div>
  );
};

export default Logout;
