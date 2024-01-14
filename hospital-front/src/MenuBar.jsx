import React from 'react';
import { Link, useLocation } from 'react-router-dom';

const MenuBar = ({ menus }) => {
  const currentPath = useLocation().pathname;

  return (
    <nav>
      <ul className='menu'>
        {menus.map((menu, index) => (
          <li key={index} className={currentPath === menu.link ? 'active' : ''}>
            <Link to={menu.link}>{menu.label}</Link>
          </li>
        ))}
      </ul>
    </nav>
  );
};

export default MenuBar;




