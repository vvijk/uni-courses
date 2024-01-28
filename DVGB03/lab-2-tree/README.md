# Lab 2: Tree Structures

## Introduction
A tree is a hierarchically ordered collection of items with ancestor-descendant relationships. The lab focuses on two lexicographically ordered tree data structures: Binary Search Tree (BST) and Adelson-Velsky-Landis (AVL) tree. Familiarize yourself with tree terminologies such as breadth-first search, inorder traversal, and perfect trees.

## Implementation
During this lab, I implemented the following:

### BST Implementation:

* Completed the Binary Search Tree (BST) methods in src/bst.py, including is_member, size, height, inorder, postorder, bfs_order_star, and delete.
* Adhered to strict implementation criteria, ensuring the height of an empty tree is zero and maintaining balance during deletion.
### User Interface (UI):

* Implemented the show_2d method in src/ui.py to print the structure and content of the tree based on bfs_order_star.
* Aimed for a "pretty 2D print" with scalability as the number of levels increases.

### AVL Tree Implementation:
* Extended the AVL tree in src/avl.py, leveraging inherited BST methods.
* Implemented AVL-specific methods: balance, slr, srr, dlr, and drr.

## Task
Getting Started
Clone the start code:

```bash
Copy code
git clone https://git.cs.kau.se/rasmoste/dvgb03.git
Run ./bin/main --help to view available options. Refer to the README.md file for additional guidance.
```
## Use available options
```
$ ./bin/main --help
usage: Terminal-based UI for BST/AVL trees [-h] [--log-level LOG_LEVEL]
                                           [--mode MODE] [--echo]


optional arguments:
  -h, --help            show this help message and exit
  --log-level LOG_LEVEL, -l LOG_LEVEL
                        Minimum verbosity for logging. Available in ascending
                        order: debug, info, warning, error, crirical.
  --mode MODE, -m MODE  Tree mode. Available options: bst, avl.
  --echo, -e            Echo input. Useful if redirecting input from file
```

As shown above, you can specify if the program should use a BST or an AVL tree.
The default is BST, and you can change it by setting the AVL mode:
- Run in BST mode: `./bin/main`
- Run in AVL mode: `./bin/main -m avl`

The echo option is useful if you want to redirect input from a file. It will
cause each input to be printed to stdout.
- Run normally: `./bin/main`
- Run with input from file: `./bin/main -e < some_input.txt`