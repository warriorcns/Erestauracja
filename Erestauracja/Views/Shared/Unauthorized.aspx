<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<script type="text/javascript">
    setTimeout(function () {
        window.location.href = "Index";
    },
3000);
</script>
<head runat="server">
    <title>Unauthorized access.</title>
</head>
<body>
    <div>
        <h1>Ups, nie masz uprawnien by obejrzeć tę stronę ;( </h1>
        <h4>Za chwilę nastąpi przekierowanie do strony początkowej...</h4>
    </div>
</body>
</html>
