using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace StUtil.Native
{
    public static class RegistryQuery
    {
        public static List<string[]> Query(List<string[]> QueryKeys)
        {
            List<string[]> QueryResults = new List<string[]>();
            while (QueryKeys.Count > 0)
            {
                string [] Q = QueryKeys.First();
                QueryKeys.RemoveAt(0);

                string regQueryRes = null;
                if (Q[0].IndexOf("*") > -1)
                {
                    string[] Qs = Q[0].Split('\\');

                    RegistryKey CurrentKey = null;
                    bool cancel = false;
                    for(int j = 0; j <Qs.Length; j++)
                    {
                        if (j == 0)
                        {
                            switch (Qs[0])
                            {
                                case "*":
                                    string path = "";
                                    for (int i = 1; i < Qs.Length; i++)
                                        path += Qs[i] + "\\";
                                    QueryKeys.Add(new string[] { "HKEY_CLASSES_ROOT\\" + path, Q[1] });
                                    QueryKeys.Add(new string[] { "HKEY_CURRENT_USER\\" + path, Q[1] });
                                    QueryKeys.Add(new string[] { "HKEY_LOCAL_MACHINE\\" + path, Q[1] });
                                    QueryKeys.Add(new string[] { "HKEY_USERS\\" + path, Q[1] });
                                    QueryKeys.Add(new string[] { "HKEY_CURRENT_CONFIG\\" + path, Q[1] });
                                    cancel = true;
                                    break;
                                case "HKEY_CLASSES_ROOT":
                                    CurrentKey = Registry.ClassesRoot;
                                    break;
                                case "HKEY_CURRENT_USER":
                                    CurrentKey = Registry.CurrentUser;
                                    break;
                                case "HKEY_LOCAL_MACHINE":
                                    CurrentKey = Registry.LocalMachine;
                                    break;
                                case "HKEY_USERS":
                                    CurrentKey = Registry.Users;
                                    break;
                                case "HKEY_CURRENT_CONFIG":
                                    CurrentKey = Registry.CurrentConfig;
                                    break;
                                default:
                                    cancel = true;
                                    break;
                            }
                            if (cancel)
                                break;
                        }
                        else
                        {
                            try
                            {
                                if (Qs[j] == "*")
                                {
                                    string path1 = "";
                                    for (int i = 0; i < j; i++)
                                        path1 += Qs[i] + "\\";
                                    string path2 = "";
                                    for (int i = j + 1; i < Qs.Length; i++)
                                        path2 += Qs[i] + "\\";
                                    foreach (string key in CurrentKey.GetSubKeyNames())
                                        QueryKeys.Add(new string[] { path1 + key + "\\" + path2, Q[1] });
                                    cancel = true;
                                } else
                                    CurrentKey = CurrentKey.OpenSubKey(Qs[j]);
                            }
                            catch (Exception)
                            {
                                //Key does not exist
                                cancel = true;
                            }
                        }
                        if (cancel)
                            break;
                    }
                }
                else
                    regQueryRes = (string)Registry.GetValue(Q[0], Q[1], null);

                if (regQueryRes != null)
                    QueryResults.Add(new string[] {Q[0] + "\\" + Q[1], regQueryRes});

            }
            
            return QueryResults;
        }

    }
}
