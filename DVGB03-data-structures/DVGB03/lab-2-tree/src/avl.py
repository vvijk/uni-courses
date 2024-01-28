#!/usr/bin/env python3

import sys
import bst
import logging

log = logging.getLogger(__name__)

class AVL(bst.BST):
    def __init__(self, value=None):
        '''
        Initializes an empty tree if `value` is None, else a root with the
        specified `value` and two empty children.
        '''
        self.set_value(value)
        if not self.is_empty():
            self.cons(AVL(), AVL())

    def add(self, v):
        '''
        Example which shows how to override and call parent methods.  You
        may remove this function and overide something else if you'd like.
        log.debug("calling bst.BST.add() explicitly from child")
        '''
        bst.BST.add(self, v)
        return self.balance()

    def delete(self, v):
        bst.BST.delete(self, v)
        return self.balance()

    def heightDiff(self):
        left = self.lc().height() if self.lc() is not None else 0
        right = self.rc().height() if self.rc() is not None else 0
        return left - right

    def balance(self):
        '''
        AVL-balances around the node rooted at `self`.  In other words, this
        method applies one of the following if necessary: slr, srr, dlr, drr.
        log.info("TODO@src/avl.py: implement balance()")
        '''
        diff = self.heightDiff()
        if diff <= -2:
            if self.rc().heightDiff() >= 1:
                return self.dlr()
            else:
                return self.slr()
        
        if diff >= 2:
            if self.lc().heightDiff() <= -1:
                return self.drr()
            else:
                return self.srr()

        return self

    def slr(self):
        '''
        Performs a single-left rotate around the node rooted at `self`.
        log.info("TODO@src/avl.py: implement slr()")
        '''
        n1 = self.rc()
        self.set_rc(n1.lc())
        n1.set_lc(self)
        return n1

    def srr(self):
        '''
        Performs a single-right rotate around the node rooted at `self`.
        log.info("TODO@src/avl.py: implement srr()")
        '''
        n1 = self.lc()
        self.set_lc(n1.rc())
        n1.set_rc(self)
        return n1

    def dlr(self):
        '''
        Performs a double-left rotate around the node rooted at `self`.
        log.info("TODO@src/avl.py: implement drl()")
        '''
        self.set_rc(self.rc().srr())
        return self.slr()

    def drr(self):
        '''
        Performs a double-right rotate around the node rooted at `self`.
        log.info("TODO@src/avl.py: implement drr()")
        '''
        self.set_lc(self.lc().slr())
        return self.srr()

if __name__ == "__main__":
    log.critical("module contains no main module")
    sys.exit(1)
