﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PagoElectronico.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace PagoElectronico.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>
    {
        public override IEnumerable<Usuario> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Usuario Get(int id)
        {
            throw new NotImplementedException();
        }

        public override int Insert(Usuario entity)
        {
            SqlCommand command = DBConnection.CreateStoredProcedure("INSERT_USUARIO");

            command.Parameters.AddWithValue("@username", entity.Username);
            command.Parameters.AddWithValue("@password", entity.HashedPassword);
            command.Parameters.AddWithValue("@pregunta_secreta", entity.PreguntaSecreta);
            command.Parameters.AddWithValue("@respuesta_secreta", entity.HashedRespuestaSecreta);
            command.Parameters.AddWithValue("@activo", entity.Activo);
            command.Parameters.AddWithValue("@habilitado", entity.Habilitado);

            return DBConnection.ExecuteNonQuery(command);
           
        }

        public void updatePassword(String username, byte[] hashedPassword)
        {
            SqlCommand command = DBConnection.CreateStoredProcedure("UPDATE_PASSWORD");

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", hashedPassword);

            DBConnection.ExecuteNonQuery(command);
        }

        public override void Delete(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public Usuario GetByUsernameAndPassword(string username, byte[] password)
        {
            RolRepository rolRepository = new RolRepository();
            Usuario usuario = null;
            bool loginSuccess = false;

            SqlCommand command = DBConnection.CreateStoredProcedure("GetUsuarioByUsernameAndPassword");
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            DataRowCollection collection = DBConnection.EjecutarStoredProcedureSelect(command).Rows;
            foreach (DataRow row in collection)
            {
                usuario = this.CreateUsuario(row);
                usuario.Roles = rolRepository.GetRolesByUsername(username);
                loginSuccess = true;

                command = DBConnection.CreateStoredProcedure("DeleteUsuarioLog");
                command.Parameters.AddWithValue("@username", username);
                DBConnection.ExecuteNonQuery(command);

            }

            command = DBConnection.CreateStoredProcedure("InsertUsuarioLog");
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@fecha", Session.Fecha);
            command.Parameters.AddWithValue("@login_correcto", loginSuccess);
            DBConnection.ExecuteNonQuery(command);

            return usuario;
        }

        private Usuario CreateUsuario(DataRow reader)
        {
            Usuario usuario = new Usuario();
            usuario.Username = reader["username"].ToString();
            usuario.Activo = Convert.ToBoolean(reader["activo"]);
            usuario.Habilitado = Convert.ToBoolean(reader["habilitado"]);
            usuario.PreguntaSecreta = reader["pregunta_secreta"].ToString();
            usuario.RespuestaSecreta = reader["respuesta_secreta"].ToString();

            return usuario;
        }
    
        public int InsertRolesUsuario(Usuario usuario)
        {
            int resultado = -1;
            
            foreach (var item in usuario.Roles)

	        {
                SqlCommand command = DBConnection.CreateStoredProcedure("INSERT_USUARIO_ROLES");

                command.Parameters.AddWithValue("@username", usuario.Username);
                command.Parameters.AddWithValue("@id_rol", item.Id);
     	        resultado =  DBConnection.ExecuteNonQuery(command);
	        }
            

            return resultado;

        }

        public bool existeUsername(string username)
        {
            SqlCommand command = DBConnection.CreateStoredProcedure("getUserByUsername");
            command.Parameters.AddWithValue("@username", username);
            if (DBConnection.EjecutarStoredProcedureSelect(command).Rows.Count > 0)
                return true;
            else
                return false;
        }

        public byte[] getPasswordHashedByUsername(string username)
        {
            SqlCommand command = DBConnection.CreateStoredProcedure("getPasswordHashedByUsername");
            command.Parameters.AddWithValue("@username", username);
            DataRow dataRow = DBConnection.EjecutarStoredProcedureSelect(command).Rows[0];
            byte[] passwordHashed = (byte[])dataRow["password"];
            return passwordHashed;
        }

        public override void Update(Usuario entity)
        {
            throw new NotImplementedException();
        }
    }
}
