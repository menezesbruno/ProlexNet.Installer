using System.IO;

namespace ProlexNetUpdater.Library.Common
{
    public static class UpdateResult
    {
        public static void Build(string htmlResult, int updateResult)
        {
            StreamWriter file = new StreamWriter(htmlResult, false);
            file.WriteLine("<!DOCTYPE html>");
            file.WriteLine("<html>");
            file.WriteLine("<head>");
            file.WriteLine("<meta charset=\"UTF-8\"/>");
            file.WriteLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\">");
            file.WriteLine("<title>ProlexNet</title>");
            file.WriteLine("<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css\">");
            file.WriteLine("<style>a,a:focus,a:hover{color:#fff}.btn-secondary,.btn-secondary:focus,.btn-secondary:hover{color:#333;text-shadow:none;background-color:#fff;border:.05rem solid #fff}body,html{height:100%;background-color:#333}body{display:-ms-flexbox;display:flex;color:#fff;text-shadow:0 .05rem .1rem rgba(0,0,0,.5);box-shadow:inset 0 0 5rem rgba(0,0,0,.5)}.cover-container{max-width:42em}.masthead{margin-bottom:2rem}.masthead-brand{margin-bottom:0}.nav-masthead .nav-link{padding:.25rem 0;font-weight:700;color:rgba(255,255,255,.5);background-color:transparent;border-bottom:.25rem solid transparent}.nav-masthead .nav-link:focus,.nav-masthead .nav-link:hover{border-bottom-color:rgba(255,255,255,.25)}.nav-masthead .nav-link+.nav-link{margin-left:1rem}.nav-masthead .active{color:#fff;border-bottom-color:#fff}@media (min-width:48em){.masthead-brand{float:left}.nav-masthead{float:right}}.cover{padding:0 1.5rem}.cover .btn-lg{padding:.75rem 1.25rem;font-weight:700}.mastfoot{color:rgba(255,255,255,.5)}</style>");
            file.WriteLine("</head>");
            file.WriteLine("<body class=\"text-center\">");
            file.WriteLine("<div class=\"cover-container d-flex w-100 h-100 p-3 mx-auto flex-column\">");
            file.WriteLine("<header class=\"masthead mb-auto\">");
            file.WriteLine("<div class=\"inner\">");
            file.WriteLine("<h3 class=\"masthead-brand\">ProlexNet</h3>");
            file.WriteLine("<nav class=\"nav nav-masthead justify-content-center\">");
            file.WriteLine("<a class=\"nav-link active\" href=\"#\">Link</a>");
            file.WriteLine("<a class=\"nav-link\" href=\"#\">Link</a>");
            file.WriteLine("<a class=\"nav-link\" href=\"#\">Link</a>");
            file.WriteLine("</nav>");
            file.WriteLine("</div>");
            file.WriteLine("</header>");
            file.WriteLine("<main role=\"main\" class=\"inner cover\">");
            if (updateResult == 0)
            {
                file.WriteLine($"<h1 class=\"cover-heading\">O ProlexNet foi atualizado com sucesso</h1>");
            }
            else
            {
                file.WriteLine($"<h1 class=\"cover-heading\">Erro durante a atualização do ProlexNet</h1>");
                file.WriteLine("<p class=\"lead\">Entre em contato com a Automatiza.</p>");
                //file.WriteLine("<img style=\"height:75px;\" src=\"data:image/svg+xml;base64, PHN2ZyBhcmlhLWhpZGRlbj0idHJ1ZSIgZGF0YS1wcmVmaXg9ImZhcyIgZGF0YS1pY29uPSJleGNsYW1hdGlvbi1jaXJjbGUiIGNsYXNzPSJzdmctaW5saW5lLS1mYSBmYS1leGNsYW1hdGlvbi1jaXJjbGUgZmEtdy0xNiIgcm9sZT0iaW1nIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCA1MTIgNTEyIj48cGF0aCBmaWxsPSJjdXJyZW50Q29sb3IiIGQ9Ik01MDQgMjU2YzAgMTM2Ljk5Ny0xMTEuMDQzIDI0OC0yNDggMjQ4UzggMzkyLjk5NyA4IDI1NkM4IDExOS4wODMgMTE5LjA0MyA4IDI1NiA4czI0OCAxMTEuMDgzIDI0OCAyNDh6bS0yNDggNTBjLTI1LjQwNSAwLTQ2IDIwLjU5NS00NiA0NnMyMC41OTUgNDYgNDYgNDYgNDYtMjAuNTk1IDQ2LTQ2LTIwLjU5NS00Ni00Ni00NnptLTQzLjY3My0xNjUuMzQ2bDcuNDE4IDEzNmMuMzQ3IDYuMzY0IDUuNjA5IDExLjM0NiAxMS45ODIgMTEuMzQ2aDQ4LjU0NmM2LjM3MyAwIDExLjYzNS00Ljk4MiAxMS45ODItMTEuMzQ2bDcuNDE4LTEzNmMuMzc1LTYuODc0LTUuMDk4LTEyLjY1NC0xMS45ODItMTIuNjU0aC02My4zODNjLTYuODg0IDAtMTIuMzU2IDUuNzgtMTEuOTgxIDEyLjY1NHoiPjwvcGF0aD48L3N2Zz4=\"/>");
                file.WriteLine("<a href=\"http://www.automatizatec.com.br/login\" class=\"btn btn-lg btn-danger\">Atendimento online</a>");
                file.WriteLine("</p>");
            }
            file.WriteLine("</main>");
            file.WriteLine("<footer class=\"mastfoot mt-auto\">");
            file.WriteLine("<div class=\"inner\">");
            file.WriteLine("<p><a href=\"http://www.automatizatec.com.br/\">Automatiza Tecnologia e Automação Ltda.</a></p>");
            file.WriteLine("</div>");
            file.WriteLine("</footer>");
            file.WriteLine("</div>");
            file.WriteLine("<script src=\"https://code.jquery.com/jquery-3.2.1.slim.min.js\"></script>");
            file.WriteLine("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js\"></script>");
            file.WriteLine("<script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js\"></script>");
            file.WriteLine("</body>");
            file.WriteLine("</html>");
            file.Close();
        }
    }
}