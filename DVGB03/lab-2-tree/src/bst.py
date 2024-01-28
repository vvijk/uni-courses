#!/usr/bin/env python3

import bt
import sys
import logging

log = logging.getLogger(__name__)

class BST(bt.BT):
    def __init__(self, value=None):
        '''
        Initializes an empty tree if `value` is None, else a root with the
        specified `value` and two empty children.
        '''
        self.set_value(value)
        if not self.is_empty():
            self.cons(BST(), BST())

    def is_member(self, v):
        '''
        Returns true if the value `v` is a member of the tree.
        logging.info("TODO@src/bst.py: implement is_member()")
        '''
        if self.is_empty():
            return False
        if v == self.value():
            return True
        if v < self.value():
            return self.lc().is_member(v)
        if v > self.value():
            return self.rc().is_member(v)

    def size(self):
        '''
        Returns the number of nodes in the tree.
        logging.info("TODO@src/bst.py: implement size()")
        '''
        if self.is_empty():
            return 0
        else:
            return self.lc().size() + self.rc().size() + 1

    def height(self):
        '''
        Returns the height of the tree.
        logging.info("TODO@src/bst.py: implement height()")
        '''
        if self.is_empty():
            return 0
        else:
            leftSide = self.lc().height()
            rightSide = self.rc().height()
            return 1 + max(leftSide, rightSide)

    def preorder(self):
        '''
        Returns a list of all members in preorder.
        '''
        if self.is_empty():
            return []
        return [self.value()] + self.lc().preorder() + self.rc().preorder()

    def inorder(self):
        '''
        Returns a list of all members in inorder.
        log.info("TODO@src/bst.py: implement inorder()")
        '''
        if self.is_empty():
            return []
        return self.lc().inorder() + [self.value()] + self.rc().inorder()

    def postorder(self):
        '''
        Returns a list of all members in postorder.
        log.info("TODO@src/bst.py: implement postorder()")
        '''
        if self.is_empty():
            return []
        return self.lc().postorder() + self.rc().postorder() + [self.value()]

    def bfs_order_star(self):
        '''
        **USE HEAP**
        Returns a list of all members in breadth-first search* order, which
        means that empty nodes are denoted by "stars" (here the value None).

        For example, consider the following tree `t`:
                    10
              5           15
           *     *     *     20

        The output of t.bfs_order_star() should be:
        [ 10, 5, 15, None, None, None, 20 ]
        '''
        if self.is_empty():         
            return []
        queue = []
        lista = []
        size = ((2**self.height())-1)

        queue.append(self)

        while len(queue) <=  size:
            node = queue.pop(0)
            lista.append(node.value())
            x = BST(0)
            x.set_value(None)
            queue.append(node.lc()) if not node.lc().is_empty() else queue.append(x) 
            queue.append(node.rc()) if not node.rc().is_empty() else queue.append(x)                     
        return lista

    def add(self, v):
        '''
        Adds the value `v` and returns the new (updated) tree.  If `v` is
        already a member, the same tree is returned without any modification.
        '''
        if self.is_empty():
            self.__init__(value=v)
            return self
        if v < self.value():
            return self.cons(self.lc().add(v), self.rc())
        if v > self.value():
            return self.cons(self.lc(), self.rc().add(v))
        return self
    
    def delete(self, v):
        '''
        Removes the value `v` from the tree and returns the new (updated) tree.
        If `v` is a non-member, the same tree is returned without modification.
        '''
        if self.value() is None:
            return self
        if v < self.value():
            return self.cons(self.lc().delete(v), self.rc())
        if v > self.value():
            return self.cons(self.lc(), self.rc().delete(v))
        if v == self.value():
            return self.remove_root(v)
        return self
        
    def maxValue(self):
        if self.rc().value() is not None:
            return self.rc().maxValue()
        else:
            return self

    def minValue(self):
        if self.lc().value() is not None:
            return self.lc().minValue()
        else:
            return self

    def remove_root(self, v):
        #4 cases
        if self.lc() is None and self.rc() is None:
            self.set_value(None)
            self.set_lc(None)
            self.set_rc(None)
            return self

        if self.lc().value() is None:
            tmp = self.rc()
            self.set_value(None)
            return tmp

        if self.rc().value() is None: 
            tmp = self.lc()
            self.set_value(None)
            return tmp

        if self.lc().value() is not None and self.rc().value() is not None:
            rightdepth = self.rc().height()
            leftdepth = self.lc().height()

            if rightdepth > leftdepth:
                new_node = self.rc().minValue()
                self.set_value(new_node.value())
                self.set_rc(self.rc().delete(new_node.value()))
            else:
                new_node = self.lc().maxValue()
                self.set_value(new_node.value())
                self.set_lc(self.lc().delete(new_node.value()))
        return self
            
if __name__ == "__main__":
    log.critical("module contains no main module")
    sys.exit(1)
