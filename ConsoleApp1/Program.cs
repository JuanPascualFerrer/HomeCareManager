using ConsoleApp1;

Data data = new Data();
string roleId = "role1";

bool insert1 = data.InsertRole(roleId, "Cuidador");
Console.WriteLine(insert1 ? "Insert correcto" : "Insert error");

bool insert2 = data.InsertRole(roleId, "Cuidador repetido");
Console.WriteLine(insert2 ? "Insert duplicado correcto" : "No deja insertar duplicados");

bool update = data.UpdateRole(roleId, "Cuidador actualizado");
Console.WriteLine(update ? "Update correcto" : "Update error");

bool delete = data.DeleteRole(roleId);
Console.WriteLine(delete ? "Delete correcto" : "Delete error");
