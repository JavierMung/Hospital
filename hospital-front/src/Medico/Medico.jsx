import React, { useState} from 'react';

const Medico = ({mediData}) => {
    const idTrabajador = (mediData);

    return (
        <div>
            Este es el componente Medico para el Trabajador con ID: {mediData.idMedico}
        </div>
    );
};

export default Medico;
