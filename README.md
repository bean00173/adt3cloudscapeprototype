# Cloudscape Prototype

### ***This project is currently in the prototyping phase***

Follow the adventures of a trio of friends from Devotio Village as they discover the unknown fate of the Lands of Anathema. Venture in to the clouds and explore roguelike towers, battling through warriors tainted by a mysterious curse.

### Using : [Unity Version 2022.3.5f1](https://unity.com/releases/editor/whats-new/2022.3.5)

*Made by Cedric Lina and Ben Mead*

## Guide to using GitHub with Unity

This is a brief explanation for project members on how to use GitHub with Unity.

You will need to ensure that [Git](https://git-scm.com/downloads) is installed on the computer.

### Daily Setup

At the beginning of each day, if an existing copy of the repository is not stored locally you can clone it down to begin working quickly.

GitBash is an application that allows you to run Git commands. It is what will be used for all of the commands listed below.

However first you must change your directory in git bash to where you would like the project to exist (e.g. Documents)
You can do so using the following command : 

```bash
cd C:/Documents
```
Once you are in the correct directory, you can then clone down the repository.
To do so you can use the git clone command in your git bash terminal:
```bash
git clone https://github.com/bean00173/adt3cloudscapeprototype
```

### Branching

It is recommended when making large scale changes to the project to first create a branch.

You can first check existing branches using the following git command :

```bash 
git branch
```
This returns a list of all the current branches.

If the branch you need already exists you can switch to it with the following :
```bash
git checkout <branchname>
```

To create and switch to a new branch, use the following : 
```bash
git checkout -b <new-branch>
```
In this code the `-b` is an argument switches the current branch to the newly created one immediately, without the need to use two separate commands.

### Making Changes

When editing the Unity Project, it is best to commit often, even with smaller changes (to a reasonable extent); i.e. little changes more often is better than big changes scarcely.

To commit your changes to the remote repository there are a series of commands that need to be executed.

First, you need to stage all of your changes with the following command : 
```bash
git add .
```
This command stages all the changes. The `.` after `add` denotes that everything will be collected.

Next is the following :
```bash
git commit -m "Describe Your Changes Here"
```
This command creates the commit, ready to be sent to the remote repository. The `-m` allows you to add a commit message without the need to open up an external text editor.

In a situation where you get to this point, before realizing you need to quickly change something small, you will need to start from the beginning, adding all the files to the staging area. But instead of creating the commit, we can amend the original commit that still exists.

We can do so via the following :
```bash
git commit --amend -m "Commit Message"
```
If you don't need to change the commit message, the original one will still be stored. In this case the command will look like this :
```bash
git commit --amend --no-edit
```

Once your commit has been created and an appropriate Commit Message has been written, you can push the changes to the repository.

Do so via the following command :
```bash
git push
```
If any errors are presented, consult Ben.

### Receiving Changes

If you are working on the same branch as someone (unadvisable), or you are on the receiving end of a merge, you will need to update your current workspace to match the remote repository.

This is a simple command process, first you need to bring your remote reference up to date. Use :
```bash
git remote update
```
Then to tell find out if you are behind or ahead, use :
```bash
git status -uno
```
If you are behind, your local repository needs to be updated. You can do so with :
```bash
git pull
```

### Merging Branches

Before Merging, it is highly important to ensure that both the receiving branch (usually main) and the merging branch are fully up to date.

The process of merging takes place in the branch that you are technically merging to, so if you are trying to merge to `main` you need to be in `main`   (see switching branches). Hence, `main` needs to be updated using a `pull` (see above).

To merge your branch into the receiving you can use the following command :
```bash
git merge <branch-to-be-merged>
```
\
\
\
***If you have any other questions about using GitHub for Unity Version Control ask Ben***


