import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, useNavigate } from 'react-router-dom';
import Login from './login';
import Menu from './MenuBar';
import Receps from './Recepcionista/Recepsionista.jsx';
import Medico from './Medico/Medico.jsx';
import Cliente from './Cliente/Cliente.jsx';
import Logout from './logout.jsx'
import Prueba from './prueba.jsx'

const App = () => {
  const navigate = useNavigate;
  const [userRole, setUserRole] = useState(1);

  const handleLogin = (role) => {
    setUserRole(role);
  };
  
  const handleLogout = () => {
    const confirmLogout = window.confirm('¿Estás seguro de que deseas cerrar sesión?');
    if (confirmLogout) {
      setUserRole(1);
      navigate('/login'); // Redirige a la página de inicio de sesión después de cerrar sesión
    }
  }

  // Define los elementos de menú según el rol
  const getMenuItems = () => {
    switch (userRole) {
      case 1:
        return [
          { label: 'Recepcionista', link: '/Recepcionista/Recepsionista' },
          { label: 'Medico', link: '/Medico/Medico' },
          { label: 'Cliente', link: '/Cliente/Cliente' },
          { label: 'IniciarSesion', link: '/login' },
        ];
      case 2:
        return [
          { label: 'Medico', link: '/Recepcionista/Recepsionista' },
          { label: 'Recepcionista', link: '/Medico/Medico' },
          { label: 'Cliente', link: '/Cliente/Cliente' },
          { label: 'Cerrar Sesion', UserActivation: handleLogout },
        ];
      case 3:
        return [
          { label: 'Recepcionista', link: '/Recepcionista/Recepsionista' },
          { label: 'Cliente', link: '/Medico/Medico' },
          { label: 'Medico', link: '/Cliente/Cliente' },
          { label: 'Cerrar Sesion', action: handleLogout },
          
        ];
      case 4:
          return [
            { label: 'Recepcionista', link: '/Recepcionista/Recepsionista' },
            { label: 'Cliente', link: '/Medico/Medico' },
            { label: 'Medico', link: '/Cliente/Cliente' },
            { label: 'Cerrar Sesion', action: handleLogout },
            
          ];
      default:
        return [];
    }
  };

  

  const menuItems = getMenuItems();
  return (
    <Router>
      <div>
        <Menu menus={menuItems} />

        {/* Definición de rutas */}
        <Routes>
          <Route path="/Recepcionista/Recepsionista" element={<Receps />} />
          <Route path="/Medico/Medico" element={<Medico />} />
          <Route path="/Cliente/Cliente" element={<Cliente />} />
          <Route path="/login" element={<Login onLogin={handleLogin}/>} />
          {/*<Route path="/logout" element={<Logout onLogout={handleLogin}/>} />*/}
        </Routes>
        
        <Prueba idTrabajador={3} />
      </div>
    </Router>
  );
};

export default App;


