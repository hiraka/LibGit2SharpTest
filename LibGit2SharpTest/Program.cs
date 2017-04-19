using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace LibGit2SharpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            LibGit2Sharp.Repository repo = new LibGit2Sharp.Repository(@"C:\Users\hiraka\Documents\Visual Studio 2015\Projects\LibGit2SharpTest");

            Debug.WriteLine("##############################");
            Debug.WriteLine(repo.Info.Path);

            // repoはオープンしたリポジトリオブジェクト
            foreach (Branch branch in repo.Branches)
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

            Branch developBranch = repo.Branches["develop"];
            Commit commit0 = developBranch.Commits.ElementAt(0);
            Commit commit1 = developBranch.Commits.ElementAt(2);


            // commitはCommitオブジェクト
            // コミットのツリー
            Tree currentTree = commit0.Tree;

            // 親コミットのうちの一つ
            Commit parentCommit = commit1;
            // 親コッミトのツリー
            Tree parentTree = parentCommit == null ? null : parentCommit.Tree;

            // 両者を比較して、ファイル差分のコレクションを得る。
            // 変数repoはオープンしたリポジトリオブジェクトです。
            TreeChanges changes = repo.Diff.Compare<LibGit2Sharp.TreeChanges>(parentTree, currentTree);

            // 各差分のパスと操作内容を取り出して文字列にし、全差分の文字列を連結する。
            // TreeChangesコレクションをSelectで文字列に加工してToArryで配列に変換し、配列をstring.Joinで連結。LINQ使ってみました。
            string diffFiles = string.Join("\n", changes.Select(x => x.Path + " " + x.Status).ToArray());

            Debug.WriteLine("##############################");
            Debug.WriteLine(diffFiles);

            Patch changesDetail = repo.Diff.Compare<Patch>(parentTree, currentTree);
            foreach (var diff in changesDetail)
            {
                string path = diff.Path; // 変更されたファイルパス
                int addLine = diff.LinesAdded; // 追加された行数
                int delLine = diff.LinesDeleted; // 削除された行数
                string diffs = diff.Patch; // 追加行、削除行などの差分情報

                Debug.WriteLine("  ------------");
                Debug.WriteLine("  Path :" + path);
                Debug.WriteLine("  Add  :" + addLine);
                Debug.WriteLine("  Del  :" + delLine);
                Debug.WriteLine("  Diff :" + diffs);
            }
        }
    }
}