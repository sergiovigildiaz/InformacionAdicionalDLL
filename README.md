# InformacionAdicionalDLL
DLL que muestra información adicional para a3ERP

# Manual de Usuario - Programación del DLL de InformacionAdicional

Este manual describe el uso y funcionamiento del DLL `InformacionAdicional` que integra funcionalidades adicionales con a3ERP.

## Tabla de Contenidos

1. [Introducción](#introducción)
2. [InformacionAdicional](#informacionadicional)
3. [AdicionalBotones](#adicionalbotones)
4. [AdicionalDocumentoUnidadesServidas](#adicionaldocumentounidadesservidas)
5. [AdicionalDocumentoCompras](#adicionaldocumentocompras)

---

## Introducción

La programación se compone de un DLL principal llamado `InformacionAdicional.cs`, que contiene tres formularios de Windows Forms:

- `AdicionalBotones.cs`
- `AdicionalDocumentoCompras.cs`
- `AdicionalDocumentoUnidadesServidas.cs`

![image](https://github.com/user-attachments/assets/85ba4b8a-eb4c-4f0f-9e16-6070766b78ae)

---

## InformacionAdicional

Esta clase gestiona la interacción con la base de datos y proporciona funcionalidades adicionales a a3ERP. Sus principales funciones incluyen:

- **Conexión a la Base de Datos**: Utiliza la cadena de conexión `conexion`.
  ![image](https://github.com/user-attachments/assets/8f1975f9-7b71-4367-8b81-c8b7bce60d57)
- **Integración con a3ERP**: Establece comunicación a través del objeto `Enlace`.
![image](https://github.com/user-attachments/assets/74c28645-51ef-46bb-9197-e4d794055d0c)
- **Gestión de Windows Forms**.
- **Eventos de documentos**: Implementa métodos como:
  - `AntesDeGuardarDocumentoV2`: Prepara datos antes de guardar documentos.
  - `DespuesDeGuardarDocumentoV2`: Configura el documento tras guardarlo.
  - `DespuesDeCargarDocumentoV2`: Muestra formularios y botones adicionales según el tipo de documento.

---

## AdicionalBotones

![image](https://github.com/user-attachments/assets/54722fa8-113b-45bf-9298-3d1dfadf42b8)

El formulario `AdicionalBotones` consta de tres botones:

- **A Pedido**
- **A Albarán**
- **A Factura**

Estos botones permiten servir el documento actual (como una oferta de venta) a un pedido, un albarán o una factura.

Dependiendo del tipo de documento, se muestran botones específicos:

- En Ofertas: se muestran los tres botones.
- En Pedidos: se muestran los botones **A Albarán** y **A Factura**.
- En Albaranes: solo se muestra el botón **A Factura**.

Esto se controla mediante los métodos `showParaPedidos()`, `showParaOfertas()` y `showParaAlbaranes()`.

![image](https://github.com/user-attachments/assets/427dfd0e-4234-42ae-91cf-9730826dc93b)

El archivo `MostrarDocumentos.txt` permite configurar qué botones mostrar en qué documentos, cambiando las líneas por `true` o `false` según sea necesario. La lógica está implementada en el método `DeberiaMostrarDocumentoBotones()`.

![image](https://github.com/user-attachments/assets/11de6a5c-04a0-489f-9f85-de1e4562200e)

---

## AdicionalDocumentoUnidadesServidas

![image](https://github.com/user-attachments/assets/04aa90c0-8aae-4718-955b-172d859fa0ad)

Este formulario muestra una tabla con los siguientes campos:

- **Descripción** del artículo.
- **Uds. Anuladas**: Unidades anuladas anteriormente.
- **Unidades**: Número total de unidades.
- **Uds. Servidas**: Unidades servidas anteriormente.
- **Unidades a Servir**: Campo para especificar cuántas unidades se desean servir.
- **Unidades a Anular**: Campo para especificar cuántas unidades se desean anular.

Botones:

- **Servir**: Sirve y anula las unidades especificadas.
- **Servir Todo**: Sirve todas las unidades de una vez.

Dependiendo del tipo de documento (por ejemplo, un albarán de compra) y la finalidad (por ejemplo, a factura), se invoca una función específica, como `servirPediVFactura()` para servir un pedido de venta a factura. También existe la opción de servir todo el documento con `ServirDocumento()`.

---

## AdicionalDocumentoCompras

![image](https://github.com/user-attachments/assets/7b543b65-14e8-40b6-b42a-5ecd657aaffe)

El formulario `AdicionalDocumentoCompras` tiene cuatro campos:

- **Bases**
- **IVA**
- **Retención**
- **Total**

Al abrir un documento, se llama al método `MostrarTotalesFacturaVenta()` para calcular los totales. Al igual que en el formulario de botones, el archivo `MostrarDocumentos.txt` permite configurar en qué documentos se muestra este formulario. La lógica está implementada en el método `adicionalDoc_Shown()`.

---

**Departamento de Software - Nea F3 Master S.L.**
