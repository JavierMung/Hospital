import React, { useState} from 'react';

const Recepsionista = ({TrabId}) => {
    const idTrabajador = (TrabId);

    return (
        <div>
            Este es el componente Recepsionista para el Trabajador con ID: {idTrabajador}
        </div>
    );
};

export default Recepsionista;