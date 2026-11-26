Archivo README.md obligatorio: En la raíz del repositorio incluye un README.md con: 
(a) erika Tamba 
y
asignatura : diseño y programación web , 
(b) instrucciones breves de ejecución ,
• 1. Interfaz y precarga (Load): Crea el formulario frmPedidoCafeteria en C# Windows Forms con los 14 controles exactos, pon ambos ListView en modo Details con FullRowSelect = true y carga los 10 productos oficiales con sus precios en lvCatalogo.
• 2. Cálculo centralizado (ActualizarTotal()): Crea un único método privado que sume los subtotales de lvPedido, agregue +$0.50 si rbtnEntrega está marcado y +$1.00 si chkPostre está marcado, mostrando el resultado en lblTotal.
• 3. Selección y validación: Al elegir un producto en lvCatalogo, muéstralo en txtProducto (ReadOnly = true) y valida en TextChanged y Validating que txtCantidad sea un entero >= 1 y que txtCliente no esté vacío antes de permitir agregar ítems.
• 4. Botones y extras: Programa CheckedChanged en las opciones para recalcular al instante, y configura los botones btnAgregar (añade fila y reinicia cantidad a "1"), btnQuitar (borra la fila elegida) y btnLimpiar (reinicia todo el formulario).
• 5. Entrega en GitHub: Sube tu proyecto limpio (excluyendo .vs, bin y obj mediante .gitignore) a tu repositorio público EAPPP_Apellido_Nombre y entrega el enlace.

y (c) listado de los eventos implementados y reglas de negocio.
• Load: Carga los 10 productos oficiales en lvCatalogo e inicializa el formulario.
• SelectedIndexChanged: Pasa el producto elegido del catálogo a txtProducto o habilita btnQuitar en el pedido.
• Validating y TextChanged: Validan en tiempo real que txtCantidad sea un entero >= 1 para habilitar btnAgregar y evitar errores.
• CheckedChanged: Actualiza el total al instante al marcar o desmarcar entrega en aula (+$0.50), postre (+$1.00) o servilletas.
• Click: Agrega ítems validados al carrito (btnAgregar), elimina filas elegidas (btnQuitar) o reinicia todo (btnLimpiar).imagenes complementarias 
