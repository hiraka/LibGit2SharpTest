using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibGit2SharpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            LibGit2Sharp.Repository repo = new LibGit2Sharp.Repository(@"");

            Debug.WriteLine("##############################");
            Debug.WriteLine(repo.Info.Path);

            // repoはオープンしたリポジトリオブジェクト
            foreach (LibGit2Sharp.Branch branch in repo.Branches)
            {
                string branchName = branch.FriendlyName; // ブランチの簡略名
                Debug.WriteLine(branchName);

                // branchはBranchオブジェクト
                foreach (LibGit2Sharp.Commit commit in branch.Commits)
                {
                    string autherName = commit.Author.Name; // 作者
                    string authorEmail = commit.Author.Email; // E-mail
                    DateTime commitTime = commit.Author.When.DateTime; // コミット日時
                    string message = commit.Message; // コミットメッセージ
                    string messageShort = commit.MessageShort; // コミットメッセージ省略形

                    Debug.WriteLine("  " + autherName + ":" + message);

                }
            }


        }
    }
}
