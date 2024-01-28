#!/usr/bin/env python3

import sys
import logging

log = logging.getLogger(__name__)

from math import inf

def warshall(adjlist):
    '''
    Returns an NxN matrix that contains the result of running Warshall's
    algorithm.

    Pre: adjlist is not empty.
    log.info("TODO: warshall()")
    '''
    matrix = floyd(adjlist)
    n = adjlist.node_cardinality()
    for i in range(n):
        matrix[i] = [False if x == inf else True for x in matrix[i]]
    return matrix

def floyd(adjlist):
    '''
    Returns an NxN matrix that contains the result of running Floyd's algorithm.

    Pre: adjlist is not empty.
    
    '''
    matrix = adjlist.adjacency_matrix()
    n = adjlist.node_cardinality()
    dis = matrix
    for k in range(n):
        for i in range(n):
            if k == i:
                dis[i][k] = 0
            for j in range(n):
                dis[i][j] = min(matrix[i][j], matrix[i][k] + matrix[k][j])
    return dis

def dijkstra(adjlist, start_node):
    d = []
    e = []
    
    unvisited = []
    head = adjlist.head()
    set_nodes_info(head, [None, inf]) #sets .info for all nodes to 2nd parameter value
    load_nodes(unvisited, head) #initializes q with adjlists nodes 
        
    #Preset the startnode values
    start_n = adjlist.get_node(start_node)
    decrease_key(unvisited, start_n, 0)
    
    #Iterate through all nodes in q. This part simply updates the .info of all nodes to build a mst
    #info()[0] is the name of the previous node, info()[1] is the weight of the edge to the chosen node
    while len(unvisited) != 0:
        a_node = extract_min(unvisited)  #extract minimal key out of q
        edge = a_node.edges()   #get edge head of node u
        while not edge.is_empty(): #iterate through all the edges
            dst_node = adjlist.get_node(edge.dst())   #get destination node of edge e
            if dst_node in unvisited and calc_key(a_node, edge)  < dst_node.info()[1]: 
                dst_node.set_info([a_node.name(), calc_key(a_node, edge)])  #update node.info
                decrease_key(unvisited, dst_node, calc_key(a_node, edge))  #update node info in q aswell
            edge = edge.tail()    #next iteration
    
    #Append node.info() to its corresponding list      
    node = adjlist.head()
    while not node.head().is_empty():
        if node.info()[1] == 0:
            d.append(None)
            e.append(None)
        else:
            d.append(node.info()[1])
            e.append(node.info()[0])
        node = node.tail()
    
    return d, e

def prim(adjlist, start_node):
    l = []
    c = []
    
    cut = []
    head = adjlist.head()
    set_nodes_info(head, [None, inf]) #sets .info for all nodes to 2nd parameter value
    load_nodes(cut, head) #initializes q with adjlists nodes 
        
    #Preset the startnode values
    start_n = adjlist.get_node(start_node)
    decrease_key(cut, start_n, 0)
    
    #Iterate through all nodes in q. This part simply updates the .info of all nodes to build a mst
    #info()[0] is the name of the previous node, info()[1] is the weight of the edge to the chosen node
    while len(cut) != 0:
        a_node = extract_min(cut)  #extract minimal key out of q
        edge = a_node.edges()   #get edge head of node u
        while not edge.is_empty(): #iterate through all the edges
            dst_node = adjlist.get_node(edge.dst())   #get destination node of edge e
            if dst_node in cut and edge.weight() < dst_node.info()[1]: 
                dst_node.set_info([a_node.name(), edge.weight()])  #update node.info
                decrease_key(cut, dst_node, edge.weight())   #update node info in q aswell
            edge = edge.tail()    #next iteration
    
    #Append ndoe.info() to its corresponding list      
    node = adjlist.head()
    while not node.head().is_empty():
        if node.info()[1] == 0:
            l.append(None)
            c.append(None)
        else:
            l.append(node.info()[1])
            c.append(node.info()[0])
        node = node.tail()
        
    return l, c

def decrease_key(q, node, key):
    for x in q:
        if x.name() == node.name():
            x.set_info([x.info()[0], key])
            break
    
def extract_min(set):
    min = inf
    min_idx = inf
    x = 0
    for i in set:
        if i.info()[1] < min:
            min = i.info()[1]
            min_idx = x
        x += 1
    
    node = set[min_idx]
    set.remove(node)
    return node


def set_nodes_info(node, key):
    while not node.head().is_empty():
        node.set_info(key)
        node = node.tail()
    
def load_nodes(list, node):
    #Initialize all nodes in list with key
    while not node.head().is_empty():
        list.append(node)
        node = node.tail()

def calc_key(node, edge):
    return node.info()[1] + edge.weight()

if __name__ == "__main__":
    logging.critical("module contains no main")
    sys.exit(1)
