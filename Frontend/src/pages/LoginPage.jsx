import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';

function LoginPage({ setIsAuthenticated }) {
  const [formData, setFormData] = useState({ correo: '', contraseña: '' });
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    setMessage('');

    try {
      const response = await axios.post('http://localhost:5000/api/auth/login', {
        correo: formData.correo,
        contraseña: formData.contraseña
      });

      localStorage.setItem('token', response.data.token);
      localStorage.setItem('user', JSON.stringify(response.data.usuario));
      setMessage('¡Login exitoso! Redirigiendo...');
      setIsAuthenticated(true);
      setTimeout(() => navigate('/dashboard'), 1500);
    } catch (err) {
      setError(err.response?.data?.message || 'Error en el login');
    } finally {
      setLoading(false);
    }
  };

  return (
    <section className="container" style={{ padding: '4rem 0' }}>
      <form onSubmit={handleSubmit}>
        <h2>Iniciar Sesión</h2>

        {error && <div className="error">{error}</div>}
        {message && <div className="success">{message}</div>}

        <div className="form-group">
          <label>Correo Electrónico</label>
          <input
            type="email"
            name="correo"
            value={formData.correo}
            onChange={handleChange}
            required
            disabled={loading}
            placeholder="tu@email.com"
          />
        </div>

        <div className="form-group">
          <label>Contraseña</label>
          <input
            type="password"
            name="contraseña"
            value={formData.contraseña}
            onChange={handleChange}
            required
            disabled={loading}
            placeholder="••••••••"
          />
        </div>

        <button
          type="submit"
          className="btn-primary"
          style={{ width: '100%' }}
          disabled={loading}
        >
          {loading ? 'Ingresando...' : 'Ingresar'}
        </button>

        <p style={{ textAlign: 'center', marginTop: '1rem', color: 'var(--gray-light)' }}>
          ¿No tienes cuenta? <Link to="/register" style={{ color: 'var(--gold)', fontWeight: 'bold' }}>Registrarse</Link>
        </p>
      </form>
    </section>
  );
}

export default LoginPage;
