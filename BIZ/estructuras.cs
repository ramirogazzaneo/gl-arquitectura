using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class estructuras
    {

        public struct usuarioCarga
        {
            public string nombre;
            public string apellido;
            public string email;
            public string telefono;
            public string contraseña;
        }

        public struct formRegistrar
        {
            public string nombre;
            public string apellido;
            public string email;
            public string telefono;
            public string contraseña;
        }

        public struct errorFormRegistrar
        {
            public string nombre;
            public string apellido;
            public string email;
            public string telefono;
            public string contraseña;
        }

        public struct errorFormLogin
        {
            public string email;
            public string contraseña;
        }

        public struct formLogin
        {
            public string email;
            public string contraseña;
        }

        public struct formPerfil
        {
            public string telefono;
            public string nombre;
            public string apellido;
        }

        public struct errorFormPerfil
        {
            public string telefono;
            public string nombre;
            public string apellido;
        }

        public struct usuarioModif
        {
            public string telefono;
            public string nombre;
            public string apellido;
        }

        public struct usuario
        {
            public string nombre { get; set; }
            public string apellido { get; set; }
            public string email { get; set; }
            public string telefono { get; set; }
            public string contraseña { get; set; }
            public int nivel { get; set; }
            public int id { get; set; }
        }

        public struct formCambiarClave
        {
            public string claveActual;
            public string claveNueva;
            public string claveConfirmacion;
        }

        public struct errorFormCambiarClave
        {
            public string claveActual;
            public string claveNueva;
            public string claveConfirmacion;
        }

        public struct Obra
        {
            public int id { get; set; }
            public string nombre { get; set; }
            public string barrio { get; set; }
            public string calle { get; set; }
            public string localidad { get; set; }
            public string provincia { get; set; }
            public string metrosT { get; set; }
            public string descripcion { get; set; }

        }

        public struct Obras
        {
            public int id { get; set; }
            public string nombre { get; set; }
            public string apellido { get; set; }
            public string nombreO { get; set; }
            public string barrio { get; set; }
            public string calle { get; set; }
            public string localidad { get; set; }
            public string provincia { get; set; }
            public string metrosT { get; set; }
            public string descripcion { get; set; }

            public int id_estado { get; set; }

        }

        public struct ObrasCarga
        {
            public int id { get; set; }
            public int id_usuario { get; set; }
            public string nombreO { get; set; }
            public string metrosT { get; set; }
            public int id_estado { get; set; }

        }

        public struct formObra
        {
            public string nombreO;
            public string metrosT;
        }
        public struct errorFormObra
        {
            public string nombreO;
            public string metrosT;
        }

        public struct direccionCarga
        {
            public int id { get; set; }
            public string calle { get; set; }
            public string numero { get; set; }
            public string barrio { get; set; }
            public string localidad { get; set; }
            public string provincia { get; set; }

        }

        public struct formDireccion
        {
            public int id { get; set; }
            public string calle { get; set; }
            public string numero { get; set; }
            public string barrio { get; set; }
            public string localidad { get; set; }
            public string provincia { get; set; }
            public string nombreO { get; set; }
            public string metrosT { get; set; }
        }

        public struct errorFormDireccion
        {
            public int id { get; set; }
            public string calle { get; set; }
            public string numero { get; set; }
            public string barrio { get; set; }
            public string localidad { get; set; }
            public string provincia { get; set; }
            public string nombreO { get; set; }
            public string metrosT { get; set; }
        }

        public struct etapaCrear
        {
            public int id_obra;   
            public string nombre;
        }

        public struct formEtapa
        {
            public int id_obra;
            public string nombre;
        }

        public struct errorformEtapa
        {
            public int id_obra;
            public string nombre;
        }

    


        public struct tareaCrear
        {
            public int id_etapa;          
            public string descripcion;
            public DateTime? fecha_inicio;
            public DateTime? fecha_fin;
            public bool es_hito;
        }

        public struct formTarea
        {
            public int id_etapa;         
            public string descripcion;
            public DateTime fecha_inicio;
            public DateTime fecha_fin;
            public bool es_hito;
        }

        public class errorformTarea
        {
            public string descripcion { get; set; }
            public string fecha_inicio { get; set; }
            public string fecha_fin { get; set; }
        }

        public struct rubroCrear
        {
            public int id_rubro;
            public string descripcion;
            public int id_obra;
        }

        public struct materialCrear
        {
            public int id_material;
            public string descripcion;
            public int id_rubro;
            public int cantidad;
            public bool entrega;
        }

        public struct formRubro
        {
            public int id_rubro;
            public string descripcion;
            public int id_obra;
        }

        public struct errorformRubro
        {
            public int id_rubro;
            public string descripcion;
            public int id_obra;
        }

        public struct fromMaterial
        {
            public int id_obra;
            public int id_etapa;
            public string descripcion;
            public DateTime fecha_inicio;
            public DateTime fecha_fin;
            public bool es_hito;
        }

        public class errorfromMaterial
        {
            public string descripcion { get; set; }
            public string fecha_inicio { get; set; }
            public string fecha_fin { get; set; }
        }

    
        public class RubroCrear
        {
            public int id_obra { get; set; }
            public string descripcion { get; set; }
        }
        public class RubroActualizar
        {
            public int id_rubroR { get; set; }
            public string descripcion { get; set; }
        }

       
        public class PresupuestoCrear
        {
        
            public int id_rubroR { get; set; }
            public decimal monto_real { get; set; }
            public string descripcion { get; set; } 
            public string valor { get; set; }
            public int id_tipo { get; set; }
        }
        public class PresupuestoActualizar
        {
            public int id_presupuesto { get; set; }
            public decimal monto_real { get; set; }
            public string descripcion { get; set; } 
        }

        public class pagoCrear
        {
            public int id_obra { get; set; }
            public int id_usuario { get; set; }
            public string descripcion { get; set; }
            public DateTime fecha { get; set; }
            public decimal monto { get; set; }
            public bool estado_pago { get; set; }

        }

        public class formPago
        {
            public int id_pago { get; set; }
            public decimal monto { get; set; }
            public string descripcion { get; set; }
        }

    }
}
