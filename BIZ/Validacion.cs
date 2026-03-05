using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BIZ.estructuras;

namespace BIZ
{
    public class Validacion
    {


        private static bool existeEmail(string email)
        {
            SqlConnection conn = A_conexion.conexionDB();
            try
            {
                string query = @"SELECT COUNT(*) FROM usuarios WHERE email = @email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                throw ex;
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        private string validateNotNullOrWhiteSpace(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "Es requerido.";
            return null;
        }
        private string validateEmailToRegister(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "El correo es requerido.";

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "El correo no tiene el formato correcto.";

            if (existeEmail(email)) return "El correo ya fue utilizado.";

            return null;
        }

        private string validateEmailToLogin(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "El correo es requerido.";

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "El correo no tiene el formato correcto.";

            return null;
        }

        private string validateEmailToRecover(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "El correo es requerido.";

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "El correo no tiene el formato correcto.";

            if (!existeEmail(email)) return "El correo no esta registrado.";

            return null;
        }

        public static string validateTelefono(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Telefono no puede estar vacio";

            if (!Regex.IsMatch(input, @"^\+[1-9]\d{7,14}$"))
                return "Telefono invalido";

            return null;
        }

        private string validateCalle(string calle)
        {
            if (string.IsNullOrWhiteSpace(calle))
                return "La calle es requerida";

            if (calle.Length > 30)
                return "El nombre no puede ser mas de 30 caracteres.";

            return null;
        }

        private string validateBarrio(string barrio)
        {
            if (string.IsNullOrWhiteSpace(barrio))
                return "El barrio es requerido";

            if (barrio.Length > 30)
                return "El nombre no puede ser mas de 30 caracteres.";

            return null;
        }

        private string validateProvincia(string provincia)
        {
            if (string.IsNullOrWhiteSpace(provincia))
                return "La provincia es requerida";

            if (provincia.Length > 50)
                return "El nombre no puede ser mas de 50 caracteres.";

            return null;
        }

        private string validateLocalidad(string localidad)
        {
            if (string.IsNullOrWhiteSpace(localidad))
                return "La localidad es requerida";

            if (localidad.Length > 30)
                return "El nombre no puede ser mas de 30 caracteres.";

            return null;
        }

        private string validateNumero(string numeroTexto)
        {
            if (string.IsNullOrWhiteSpace(numeroTexto))
                return "El número es requerido.";

          
            if (!int.TryParse(numeroTexto, NumberStyles.None, CultureInfo.InvariantCulture, out var numero))
                return "El número solo puede contener dígitos (0-9).";

           
            if (numero <= 0)
                return "El número debe ser mayor que 0.";

            return null; 
        }

        public static string validateNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return "El nombre es requerido";

            if (nombre.Length > 30)
                return "El nombre no puede ser mas de 30 caracteres.";

            return null;
        }

        private string validateApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                return "El apellido es requerido.";

            if (apellido.Length > 30)
                return "El apellido no puede ser mas de 30 caracteres.";

            return null;
        }


        public string validateContraseña(string contraseña)
        {
            if (string.IsNullOrWhiteSpace(contraseña))
                return "La contraseña es requerida.";

            if (!Regex.IsMatch(contraseña, @"^(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).+$"))
                return "La contraseña debe contener una letra mayuscula, un numero y un caracter especial.";

            return null;
        }

        public string ValidateFechaInicio(DateTime fechaI)
        {
            var hoy = DateTime.Now.Date;
            return (fechaI >= hoy) ? null : "La fecha de inicio debe ser mayor a la fecha actual.";
        }

        public bool validateMontoPositivo(string s, out decimal monto)
        {
            monto = 0m;
            var esAR = new CultureInfo("es-AR");
            s = s?.Trim();

            if (decimal.TryParse(s, NumberStyles.Number, esAR, out var m) ||
                decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out m))
            {
                if (m > 0) { monto = m; return true; }
            }
            return false;
        }

        public static bool ValidateFechaInicioo(DateTime fechaI)
        {
            var hoy = DateTime.Now.Date;
            return (fechaI >= hoy) ? true : false;
        }


        public string ValidateFechaFinalizacion(DateTime fechaInicio, DateTime fechaFin)
    {
        var hoy = DateTime.Now.Date;
        if (fechaFin <= hoy) return "La fecha de finalización debe ser mayor a la fecha actual.";
        if (fechaFin < fechaInicio) return "La fecha de finalización no puede ser anterior a la fecha de inicio.";
        return null;
    }

    public string validateLevel(string level)
        {
            if (string.IsNullOrWhiteSpace(level))
                return "El nivel es requerido.";
            if (level.Length > 30)
                return "El nivel no puede ser mas de 30 caracteres.";
            return null;
        }
        public errorFormRegistrar validarFormularioRegistrar(formRegistrar input)
        {
            errorFormRegistrar errors = new errorFormRegistrar
            {
                email = validateEmailToRegister(input.email),
                telefono = validateTelefono(input.telefono),
                nombre = validateNombre(input.nombre),
                apellido = validateApellido(input.apellido),
                contraseña = validateContraseña(input.contraseña)
            };

            return errors;
        }

        public errorFormLogin validarFormularioLogin(formLogin input)
        {
            errorFormLogin errors = new errorFormLogin
            {
                email = validateEmailToLogin(input.email),
                contraseña = validateNotNullOrWhiteSpace(input.contraseña)
            };

            return errors;
        }

        public errorFormPerfil validarFormularioPerfil(formPerfil input)
        {
            errorFormPerfil errors = new errorFormPerfil
            {
                telefono = validateTelefono(input.telefono),
                nombre = validateNombre(input.nombre),
                apellido = validateApellido(input.apellido),
            };

            return errors;
        }

        public errorFormCambiarClave validarFormCambiarClave(formCambiarClave input, string email)
        {
            var resultado = A_Userr.ValidarContrasenia(email, input.claveActual);

            errorFormCambiarClave errores = new errorFormCambiarClave
            {
                claveActual = resultado.esValido ? null : "La clave no coincide con la actual",
                claveNueva = validateContraseña(input.claveNueva),
                claveConfirmacion = input.claveConfirmacion != input.claveNueva ? "Las claves deben coincidir" : null,

            };
            return errores;
        }

        public errorFormDireccion validarDireccion(formDireccion input)
        {
            errorFormDireccion errors = new errorFormDireccion
            {
                calle = validateCalle(input.calle),
                numero = validateNumero(input.numero),
                barrio = validateBarrio(input.barrio),
                provincia = validateProvincia(input.provincia),
                localidad = validateLocalidad(input.localidad),
            };

            return errors;

        }

        public errorFormObra validarObra(formObra input)
        {
            errorFormObra errors = new errorFormObra
            {
                nombreO = validateNombre(input.nombreO),
                metrosT = validateNumero(input.metrosT)
            };

            return errors;

        }

        public errorformTarea validarTarea(formTarea input)
        {
            errorformTarea errors = new errorformTarea
            {
                descripcion = validateNombre(input.descripcion), 
                fecha_inicio = ValidateFechaInicio(input.fecha_inicio),
                fecha_fin = ValidateFechaFinalizacion(input.fecha_inicio, input.fecha_fin)
            };
            return errors;
        }

        public static bool ValidoMonto(string s, out decimal monto)
        {
            monto = 0m;
            var esAR = new CultureInfo("es-AR");
            s = (s ?? "").Trim();

            if (decimal.TryParse(s, NumberStyles.Number, esAR, out var m) ||
                decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out m))
            {
                if (m > 0) { monto = m; return true; }
            }
            return false;
        }

        public static bool ValidoFechaI(string fechaStr)
        {
            DateTime fechaValida = DateTime.MinValue;

            if (DateTime.TryParse(fechaStr, out DateTime f))
            {
                if (f.Date > DateTime.Today)
                {
                    fechaValida = f;
                    return true; 
                }
            }
            return false; 
        }

    }
}
