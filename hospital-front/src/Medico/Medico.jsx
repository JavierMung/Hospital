import React, { useState} from 'react';

const Medico = ({TrabId}) => {
    const idTrabajador = (TrabId);

    return (
        <div>
            Este es el componente Medico para el Trabajador con ID: {idTrabajador}
        </div>
    );
};

export default Medico;
