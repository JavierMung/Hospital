import React, { useState, createContext, useEffect} from 'react';
import { BrowserRouter as Router, Routes, Route, useNavigate } from 'react-router-dom';
import Login from './Login.jsx';
import Menu from './MenuBar';
import Receps from './Recepcionista/Recepsionista.jsx';
import Medico from './Medico/Medico.jsx';
import Cliente from './Cliente/Cliente.jsx';
import Logout from './Logout.jsx';
import Prueba from './prueba.jsx';
import Inicio from './Inicio.jsx';
import Admin from './Admi/Administrador.jsx';

const App = () => {
  const navigate = useNavigate;
  const [userRole, setUserRole] = useState(0);
  const TrabajadorId = createContext(null);
  const [user, setUser] = useState({
    idRol: 0,
    username: null,
    idTrabajador: null,
  });

  useEffect(() => {
    // Recuperar el estado del usuario desde localStorage al cargar la aplicación
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
  }, []);

  const handleUser = ({ idRol, username, idTrabajador }) => {
    setUser({ idRol, username, idTrabajador });
    // Guardar el estado del usuario en localStorage
    localStorage.setItem('user', JSON.stringify({ idRol, username, idTrabajador }));
  };

  const handleLogout = (role) => {
    setUser({ idRol: role, username: null, idTrabajador: null });
    // Eliminar el estado del usuario de localStorage al cerrar sesión
    localStorage.removeItem('user');
  };
  

  // Define los elementos de menú según el rol
  const getMenuItems = () => {
    switch (user.idRol) {
      case 0:
        return [
          //Menu para Cliente (Por defecto)
          { label: 'Inicio', link: '/Inicio' },
          { label: 'Cliente', link: '/Cliente/Cliente' },
          { label: 'IniciarSesion', link: '/Login' },
        ];
      case 1:
        return [
          //Menu para Administrador
          { label: 'Inicio', link: '/Inicio' },
          { label: `Administrar`, link: '/Admi/Administrador' },
          { label: `${user.username}`, /*link: '/' */},
          { label: 'Cerrar Sesion', link: '/Logout' },
        ];
      case 3:
          return [
            //Menu para Recepcionista
            { label: 'Inicio', link: '/Inicio' },
            { label: 'Recepcionista', link: '/Recepcionista/Recepsionista' },
            { label: `${user.username}`, /*link: '/' */},
            { label: 'Cerrar Sesion', link: '/Logout' },
            
          ];
      case 5:
          return [
            //Menu para Medico
            { label: 'Inicio', link: '/Inicio' },
            { label: `Medico`, link: '/Medico/Medico' },
            { label: `${user.username}`, /*link: '/' */},
            { label: 'Cerrar Sesion', link: '/Logout' },
            
          ];
      default:
        return [];
    }
  };

  

  const menuItems = getMenuItems();
  return (
    <Router>
      <div>
        <TrabajadorId.Provider value ={user.idTrabajador}>
          <Menu menus={menuItems} />
          {/* Definición de rutas */}
          <Routes>
            <Route path="/Inicio" element={<Inicio />} />
            <Route path="/Admi/Administrador" element={<Admin TrabId={user.idTrabajador}/>} />
            <Route path="/Recepcionista/Recepsionista" element={<Receps TrabId={user.idTrabajador}/>} />
            <Route path="/Medico/Medico" element={<Medico TrabId={user.idTrabajador}/>} />
            <Route path="/Cliente/Cliente" element={<Cliente />} />
            <Route path="/Login" element={<Login onLogin={handleUser}/>} />
            <Route path="/Logout" element={<Logout onLogout={handleLogout}/>} />
          </Routes>

          {/*<Prueba idTrabajador={3}/>*/}
        </TrabajadorId.Provider>
          
        
      </div>
    </Router>
  );
};

export default App;
//export { TrabajadorId };


