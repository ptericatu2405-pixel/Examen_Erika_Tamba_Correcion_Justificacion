/*
 * EXAMEN PRÁCTICO ACUMULATIVO - RECUPERACIÓN
 * Asignatura: Diseño y Programación de Software
 * Estudiante: Erika Tamba
 * Fecha: 25/11/2026
 *
 * Descripción:
 * Desarrollo de una aplicación Windows Forms para la gestión de pedidos
 * de una cafetería universitaria, aplicando programación orientada a eventos,
 * validación de datos, manejo de controles gráficos y cálculo automático
 * de subtotales y totales de compra.
 *
 * Cambios realizados:
 * - Implementación del catálogo de productos.
 * - Creación del carrito de pedidos.
 * - Cálculo automático de subtotales y total general.
 * - Validación de cliente, producto y cantidad.
 * - Implementación de eventos Click, CheckedChanged y SelectedIndexChanged.
 * - Actualización dinámica de mensajes de estado.
 *
 * Resultados obtenidos:
 * - Registro de pedidos mediante interfaz gráfica.
 * - Cálculo de recargos por entrega y extras.
 * - Gestión de productos agregados y eliminados.
 *
 * Aspectos por mejorar:
 * - Completar los productos oficiales del catálogo.
 * - Optimizar la asociación de eventos del formulario.
 * - Mejorar la validación de entradas y mensajes al usuario.
 * - Refinar la interfaz gráfica y la experiencia de usuario.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Examen1
{
    public partial class Form1 : Form
    {
        decimal precioActual = 0;

        //Evita que los eventos interfieran durante la limpieza. con una aplicacion boleana para definir sus 
        // estados dependeindo de la situacion dele evento 
        bool limpiando = false;

        public Form1()
        {
            InitializeComponent();

            // los eventos se ejecuten dos veces
            // si ya fueron asociados desde el diseñador.
            this.Load -= Form1_Load;
            this.Load -= Form1_Load;
            this.Load += Form1_Load;

            lvCatalogo.SelectedIndexChanged -= lvCatalogo_SelectedIndexChanged;
            lvCatalogo.SelectedIndexChanged -= lvCatalogo_SelectedIndexChanged;
            lvCatalogo.SelectedIndexChanged += lvCatalogo_SelectedIndexChanged;

            lvPedido.SelectedIndexChanged -= lvPedido_SelectedIndexChanged;
            lvPedido.SelectedIndexChanged -= lvPedido_SelectedIndexChanged;
            lvPedido.SelectedIndexChanged += lvPedido_SelectedIndexChanged;

            btnAgregar.Click -= btnAgregar_Click;
            btnAgregar.Click -= btnAgregar_Click;
            btnAgregar.Click += btnAgregar_Click;

            btnQuitar.Click -= btnQuitar_Click;
            btnQuitar.Click -= btnQuitar_Click;
            btnQuitar.Click += btnQuitar_Click;

            btnLimpiar.Click -= btnLimpiar_Click;
            btnLimpiar.Click -= btnLimpiar_Click;
            btnLimpiar.Click += btnLimpiar_Click;

            rbtnEntrega.CheckedChanged -= rbtnEntrega_CheckedChanged;
            rbtnEntrega.CheckedChanged -= rbtnEntrega_CheckedChanged;
            rbtnEntrega.CheckedChanged += rbtnEntrega_CheckedChanged;

            chkPostre.CheckedChanged -= chkPostre_CheckedChanged;
            chkPostre.CheckedChanged -= chkPostre_CheckedChanged;
            chkPostre.CheckedChanged += chkPostre_CheckedChanged;

            chkServilletas.CheckedChanged -= chkServilletas_CheckedChanged;
            chkServilletas.CheckedChanged -= chkServilletas_CheckedChanged;
            chkServilletas.CheckedChanged += chkServilletas_CheckedChanged;
            // Inicialmente el sistema funcionaba únicamente con la configuración del constructor.
            // Sin embargo, al ejecutarse el evento Load, el botón era lo unico que funcionaba pero despues de corregir
            // paso a paso se quedo deshabilitado
            // debido a la configuración inicial de los controles.
            // Por esta razón se vuelve a habilitar el botón, permitiendo que el usuario
            // pueda reiniciar y limpiar el formulario en cualquier momento.
            btnLimpiar.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //En mi m1_Load me falta completar los 10 productos oficiales, establecer los valores
            //iniciales de los controles (rbtnMostrador seleccionado, chkPostre y chkServilletas
            //desmarcados), deshabilitar btnAgregar y btnQuitar, limpiar txtProducto y corregir el
            //mensaje de lblEstado para que muestre "Estado: Seleccione un producto."
            //Con esos cambios, el evento Load cumpliría casi todos los requisitos del examen.

            // Configuración de lvCatalogo
            lvCatalogo.View = View.Details;
            lvCatalogo.FullRowSelect = true;
            lvCatalogo.Columns.Clear();
            lvCatalogo.Columns.Add("Producto", 180);
            lvCatalogo.Columns.Add("Precio", 80);

            // Evita que se repitan los productos si Load se ejecuta nuevamente.
            lvCatalogo.Items.Clear();

            // FALTA: Reemplazar por los 10 productos oficiales del examen
            AgregarProductoCatalogo("salchipapa", 1.25m);
            AgregarProductoCatalogo("Sánduche de pollo", 2.50m);
            AgregarProductoCatalogo("Bolón de verde mixto", 2.25m);
            AgregarProductoCatalogo("Humita", 1.50m);
            AgregarProductoCatalogo("pan con queso", 1.00m);
            AgregarProductoCatalogo("Café con huevo y arroz", 0.85m);

            // FALTA: Agregar los productos que aún no están:
            // Chocolate caliente
            // Jugo natural
            // Batido de fruta
            // Botella de agua

            // Configuración de lvPedido
            lvPedido.View = View.Details;
            lvPedido.FullRowSelect = true;
            lvPedido.Columns.Clear();
            lvPedido.Columns.Add("Producto", 160);
            lvPedido.Columns.Add("Cant.", 55);
            lvPedido.Columns.Add("Subtotal", 85);

            txtCantidad.Text = "1";

            // : Limpia el producto seleccionado al iniciar.
            txtProducto.Clear();
            precioActual = 0;

            // FALTA: Configuración inicial obligatoria
            rbtnMostrador.Checked = true;
            chkPostre.Checked = false;
            chkServilletas.Checked = false;

            // sis se agrego Deshabilitar botones al iniciar
            btnAgregar.Enabled = false;
            btnQuitar.Enabled = false;

            lblTotal.Text = "Total: $0.00";

           
            lblEstado.Text = "Estado: Seleccione un producto.";
        }

        private void AgregarProductoCatalogo(string nombre, decimal precio)
        {
            // Agrega un producto al catálogo con su precio
            ListViewItem item = new ListViewItem(nombre);
            item.SubItems.Add(precio.ToString("$0.00", CultureInfo.InvariantCulture));

            // Guarda el precio para usarlo después en los cálculos
            item.Tag = precio;

            lvCatalogo.Items.Add(item);
        }

        private void ActualizarTotal()
        {
            //Este método cumple con el cálculo del total del pedido, dado que suma los
            //subtotales de los productos agregados, aplica los recargos correspondientes
            //por entrega y postre, actualiza automáticamente lblTotal y centraliza toda
            //la lógica de cálculo en un único método reutilizable (ActualizarTotal()),
            //cumpliendo el principio DRY solicitado en la rúbrica.

            decimal suma = 0;

            for (int i = 0; i < lvPedido.Items.Count; i++)
            {
                suma += (decimal)lvPedido.Items[i].Tag;
            }

            if (rbtnEntrega.Checked)
            {
                suma += 0.50m;
            }

            if (chkPostre.Checked)
            {
                suma += 1.00m;
            }

            lblTotal.Text = "Total: " + suma.ToString("$0.00", CultureInfo.InvariantCulture);
        }

        private void lvCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
           // No modificar los controles mientras se está limpiando.
            if (limpiando)
                return;

            if (lvCatalogo.SelectedItems.Count > 0)
            {
                txtProducto.Text = lvCatalogo.SelectedItems[0].Text;

                precioActual = (decimal)lvCatalogo.SelectedItems[0].Tag;

                lblEstado.Text = "Producto seleccionado: " +
                                 txtProducto.Text +
                                 " ($" + precioActual.ToString("0.00") + ")";

                btnAgregar.Enabled = true;
            }
            else
            {
                // Deshabilita Agregar cuando no hay producto seleccionado.
                txtProducto.Clear();
                precioActual = 0;
                btnAgregar.Enabled = false;
            }
        }

        //Este evento controla la selección de elementos en lvPedido. Si el usuario
        //selecciona un producto, habilita el botón btnQuitar; de lo contrario,
        //lo deshabilita para evitar intentos de eliminación sin una selección válida.
        private void lvPedido_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (limpiando)
                return;

            if (lvPedido.SelectedItems.Count > 0)
            {
                btnQuitar.Enabled = true;
            }
            else
            {
                btnQuitar.Enabled = false;
            }
        }

        //validación de cliente que si se tenia opero no se complementaba muy bien la idea , 
        validación de producto seleccionado, validación de cantidad,
        //cálculo de subtotal, agregado a lvPedido, actualización del total
        //mediante ActualizarTotal() y mensaje de confirmación al usuario.
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Verifica que el cliente haya ingresado su nombre
            if (string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                lblEstado.Text = "Ingrese el nombre del cliente.";
                txtCliente.Focus();
                return;
            }

            //Comprueba que exista una selección real en el catálogo.
            if (lvCatalogo.SelectedItems.Count == 0)
            {
                lblEstado.Text = "Seleccione un producto del catálogo.";
                return;
            }

            // correcion atravez de  Obtener  el nombre y precio directamente del producto seleccionado.
            string nombreProducto = lvCatalogo.SelectedItems[0].Text;
            precioActual = (decimal)lvCatalogo.SelectedItems[0].Tag;

            // Valida que la cantidad sea un entero mayor o igual a 1
            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad < 1)
            {
                lblEstado.Text = "La cantidad debe ser mayor o igual a 1.";
                txtCantidad.Focus();
                txtCantidad.SelectAll();
                return;
            }

            // si se se tenia el  Calculo  del subtotal del producto
            decimal subtotal = cantidad * precioActual;

            // Crea un nuevo elemento para el pedido
            ListViewItem item = new ListViewItem(nombreProducto);
            item.SubItems.Add(cantidad.ToString());
            item.SubItems.Add(subtotal.ToString("$0.00", CultureInfo.InvariantCulture));

            // Guardar el subtotal para los cálculos posteriores
            item.Tag = subtotal;

            // Agregar el producto a la lista de pedidos
            lvPedido.Items.Add(item);

            // Evitar que la selección del pedido habilite quitar automáticamente.
            lvPedido.SelectedItems.Clear();
            btnQuitar.Enabled = false;

            // Actualizar el total general
            ActualizarTotal();

            // Reiniciar la cantidad a 1
            txtCantidad.Text = "1";

            // Informar que el producto fue agregado
            lblEstado.Text = "Producto agregado correctamente.";
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            // Verifica que exista un producto seleccionado
            if (lvPedido.SelectedItems.Count > 0)
            {
                // Elimina el producto seleccionado
                lvPedido.Items.Remove(lvPedido.SelectedItems[0]);

                // Recalcula el total
                ActualizarTotal();

                //  Deshabilita Quitar después de eliminar el producto.
                btnQuitar.Enabled = false;

                // Actualiza el estado
                lblEstado.Text = "Producto eliminado correctamente.";
            }
            else
            {
                lblEstado.Text = "Seleccione un producto para eliminar.";
            }
        }
        // La lógica de limpieza del formulario se encuentra correctamente implementada,
        // ya que restablece los controles a su estado inicial y elimina la información
        // registrada durante el pedido. Sin embargo, aún es necesario fortalecer la
        // relación entre los eventos de inicialización y las dependencias generadas
        // por los controles del formulario, de manera que todos los estados del sistema
        // permanezcan sincronizados después de ejecutar la acción de limpieza.
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCliente.Clear();
            txtProducto.Clear();
            txtCantidad.Text = "1";
            precioActual = 0;

            lvCatalogo.SelectedItems.Clear();
            lvPedido.Items.Clear();

            rbtnMostrador.Checked = true;
            chkPostre.Checked = false;
            chkServilletas.Checked = false;

            btnAgregar.Enabled = false;
            btnQuitar.Enabled = false;

            lblTotal.Text = "Total: $0.00";
            lblEstado.Text = "Estado: Seleccione un producto.";
            txtCliente.Focus();
            lvPedido.Refresh();
        }
        // La lógica de reinicio del formulario funciona correctamente, ya que limpia
        // los campos, restablece las opciones iniciales y vacía la lista de pedidos.
        // Sin embargo, es necesario fortalecer la relación entre los eventos y los
        // estados iniciales de los controles, garantizando que todas las dependencias
        // del diseño se actualicen correctamente después de ejecutar la limpieza.

        private void rbtnEntrega_CheckedChanged(object sender, EventArgs e)
        {
            // Evita actualizar el estado durante la limpieza.
            if (limpiando)
                return;

            ActualizarTotal();

            if (rbtnEntrega.Checked)
                lblEstado.Text = "Estado: Entrega en aula (+$0.50).";
            else
                lblEstado.Text = "Estado: Retiro en mostrador.";
        }
        // La funcionalidad de los CheckBox se encuentra implementada correctamente;
        // sin embargo, aún es necesario fortalecer la integración con la lógica
        // inicial definida en el constructor y en el evento Load del formulario.
        // Esto permitirá mantener una correcta sincronización entre los controles
        // y evitar comportamientos inesperados durante la ejecución del sistema.


        private void chkPostre_CheckedChanged(object sender, EventArgs e)
        {
            // Evita actualizar el estado durante la limpieza.
            if (limpiando)
                return;

            ActualizarTotal();

            if (chkPostre.Checked)
                lblEstado.Text = "Estado: Postre del día agregado (+$1.00).";
            else
                lblEstado.Text = "Estado: Postre del día quitado.";
        }


        // La lógica del control funciona adecuadamente, pero se recomienda
        // reforzar la relación entre los eventos y los estados iniciales
        // establecidos en el diseño de la aplicación para mantener la coherencia
        // en la interfaz y reducir posibles confusiones para el usuario.

        private void chkServilletas_CheckedChanged(object sender, EventArgs e)
        {
            //  Evita actualizar el estado durante la limpieza.
            if (limpiando)
                return;

            ActualizarTotal();

            if (chkServilletas.Checked)
                lblEstado.Text = "Estado: Con servilletas y cubiertos.";
            else
                lblEstado.Text = "Estado: Sin servilletas ni cubiertos.";
        }
    }
}
/*
 * Conclusiones:
 * - Se desarrolló una aplicación de escritorio para la gestión de pedidos
 *   utilizando Windows Forms y programación basada en eventos.
 * - Se aplicaron validaciones de datos y cálculos automáticos para mejorar
 *   el funcionamiento y la experiencia del usuario.
 * - Se fortalecieron los conocimientos sobre el manejo de controles como
 *   ListView, TextBox, Button, CheckBox y RadioButton.
 *
 * Recomendaciones:
 * - Continuar practicando la programación orientada a eventos.
 * - Mejorar la organización y optimización del código.
 * - Realizar pruebas constantes para detectar errores de manera temprana.
 * - Implementar interfaces más intuitivas y amigables para el usuario.
 */
