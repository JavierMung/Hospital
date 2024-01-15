import React, { useState} from 'react';

const Administrador = ({TrabId}) => {
    const idTrabajador = (TrabId);

    return (
        <div>
            Este es el componente Administrador para el Trabajador con ID: {idTrabajador}
        </div>
    );
};

export default Administrador;