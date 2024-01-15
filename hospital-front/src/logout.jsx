import React from 'react';
import { useNavigate } from 'react-router-dom';

const Logout = ({ onLogout }) => {
  const navigate = useNavigate();

  const handleLogout = () => {
    onLogout(1); // Función para manejar el cierre de sesión, podría ser similar a handleLogin
    navigate('/login'); // Redirige a la página de inicio de sesión después de cerrar sesión
  };

  return (
    <div>
      <h2>Estas seguro que deseas cerrar sesion?</h2>
      <button onClick={handleLogout}>Confirmar</button>
    </div>
  );
};

export default Logout;
