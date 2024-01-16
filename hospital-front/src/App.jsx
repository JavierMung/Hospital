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
import CreaM from './Admi/CrearMedico.jsx'

const App = () => {
  const navigate = useNavigate;
  const [userRole, setUserRole] = useState(0);
  const TrabajadorId = createContext(null);
  const [user, setUser] = useState({
    idRol: 0,
    username: null,
    idTrabajador: null,
  });
  const [medicData, setMedicData] = useState({
    idTrabajador: null,
    idMedico: null,
  });

  useEffect(() => {
    // Recuperar el estado del usuario desde localStorage al cargar la aplicación
    const storedUser = localStorage.getItem('user');
    const storedMedic = localStorage.getItem('medicData')
    if (storedUser) {
      setUser(JSON.parse(storedUser));
      if (storedMedic) {
        setMedicData(JSON.parse(storedMedic));
      }
    }
  }, []);

  const obtenerIdMedico = async (idTrabajador) => {
    try {
      const response = await fetch(`https://localhost:7079/Medicos/obtenerCitasByTrabajadorId/${idTrabajador}`);
      const data = await response.json();
  
      if (response.ok && data.status === 200) {
        return data.model.idMedico;
      } else {
        console.error('Error al obtener el id del médico:', data.message);
        return null;
      }
    } catch (error) {
      console.error('Error al obtener el id del médico:', error);
      return null;
    }
  };
  

  const handleUser = ({ idRol, username, idTrabajador }) => {
    setUser({ idRol, username, idTrabajador });
    // Guardar el estado del usuario en localStorage
    localStorage.setItem('user', JSON.stringify({ idRol, username, idTrabajador }));

    // Verificar si el rol es médico (idRol igual a 5)
    if (idRol === 5) {
      // Obtener el id del médico y almacenarlo en el estado
      obtenerIdMedico(idTrabajador)
        .then((idMedico) => {
          if (idMedico !== null) {
            // Guardamos el id del médico y el id del trabajador en el estado
            setMedicData({ idTrabajador, idMedico });
            localStorage.setItem('medicData', JSON.stringify({ idTrabajador, idMedico }));
          }
        })
        .catch((error) => console.error('Error al obtener el id del médico:', error));
    }
  };

  const handleLogout = (role) => {
    setUser({ idRol: role, username: null, idTrabajador: null });
    // Eliminar el estado del usuario de localStorage al cerrar sesión
    localStorage.removeItem('user');
    localStorage.removeItem('medicData');
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
          { label: `Dar de alta Medico`, link: '/Admi/CrearMedico' },
          { label: `Administrar Medicos`, link: '/Admi/Administrador' },
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
            { label: `hola ${medicData.idMedico}`, /*link: '/' */},
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
            <Route path="/Admi/CrearMedico" element={<CreaM TrabId={user.idTrabajador}/>} />
            <Route path="/Admi/Administrador" element={<Admin TrabId={user.idTrabajador}/>} />
            <Route path="/Recepcionista/Recepsionista" element={<Receps TrabId={user.idTrabajador}/>} />
            <Route path="/Medico/Medico" element={<Medico mediData={medicData}/>} />
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


