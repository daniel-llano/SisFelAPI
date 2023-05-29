
using System.Net;
using System.Net.Mail;
 
namespace Correos
{
     class EnviadorCorreos
    {
        string Emisor = ""; //de quien procede, puede ser un alias
        string Receptor;  //a quien vamos a enviar el mail
        string Mensaje;  //mensaje
        string Asunto; //asunto
        List<string> Archivo = new List<string>(); //lista de archivos a enviar
        string DE = "correopruebascosett@gmail.com"; //nuestro usuario de smtp
        string PASS = "amrtlctaddzmgwer"; //nuestro password de smtp
 
        System.Net.Mail.MailMessage Email;
 
        public string error = "";
 
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Emisor">Procedencia</param>
        /// <param name="Para">Mail al cual se enviara</param>
        /// <param name="Mensaje">Mensaje del mail</param>
        /// <param name="Asunto">Asunto del mail</param>
        /// <param name="ArchivoPedido_">Archivo a adjuntar, no es obligatorio</param>
        public EnviadorCorreos(string Emisor, string Receptor, string Mensaje, string Asunto, List<string> ArchivoPedido_ = null)
        {
            this.Emisor = Emisor;
            this.Receptor = Receptor;
            this.Mensaje = Mensaje;
            this.Asunto = Asunto;
            Archivo = ArchivoPedido_;
        }
 
        /// <summary>
        /// metodo que envia el mail
        /// </summary>
        /// <returns></returns>

        public bool enviaMail()
        {

           
 
            //una validación básica
            if (Receptor.Trim().Equals("") || Mensaje.Trim().Equals("") || Asunto.Trim().Equals(""))
            {
                error = "El mail, el asunto y el mensaje son obligatorios";
                return false;
            }
 
            //aqui comenzamos el proceso
            //comienza-------------------------------------------------------------------------
            try
            {
                //creamos un objeto tipo MailMessage
                //este objeto recibe el sujeto o persona que envia el mail,
                //la direccion de procedencia, el asunto y el mensaje
                Email = new System.Net.Mail.MailMessage(Emisor, Receptor, Asunto, Mensaje);
 
                //si viene archivo a adjuntar
                //realizamos un recorrido por todos los adjuntos enviados en la lista
                //la lista se llena con direcciones fisicas, por ejemplo: c:/pato.txt
                if (Archivo != null)
                {
                    //agregado de archivo
                    foreach (string archivo in Archivo)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (System.IO.File.Exists(@archivo))
                        {
                            Email.Attachments.Add(new Attachment(@archivo));
                        }
                            
 
                    }
                }
 
                Email.IsBodyHtml = true; //definimos si el contenido sera html
                Email.From = new MailAddress(Emisor); //definimos la direccion de procedencia
 
                //aqui creamos un objeto tipo SmtpClient el cual recibe el servidor que utilizaremos como smtp
                //en este caso me colgare de gmail
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("smtp.gmail.com");
 
                smtpMail.EnableSsl = true;//le definimos si es conexión ssl
                smtpMail.UseDefaultCredentials = false; //le decimos que no utilice la credencial por defecto
                smtpMail.Host = "smtp.gmail.com"; //agregamos el servidor smtp
                smtpMail.Port = 587; //le asignamos el puerto, en este caso gmail utiliza el 465
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS); //agregamos nuestro usuario y pass de gmail
 
                //enviamos el mail
                smtpMail.Send(Email);
 
                //eliminamos el objeto
                Email.Dispose();
                smtpMail.Dispose();
 
                //regresamos true
                return true;
            }
            catch (Exception ex)
            {
                //si ocurre un error regresamos false y el error
                error = "Ocurrio un error: " + ex.Message;
                Console.WriteLine(ex.Message);
                return false;
            }
 
            return false;
 
        }
    }
}