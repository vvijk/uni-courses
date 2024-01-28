#include "algorithm.h"
#include <stdio.h>

//
// Private
//

//
// Public
//
void swap(int *ptr1, int *ptr2) {
    int temp = *ptr1;
    *ptr1 = *ptr2;
    *ptr2 = temp;
}

void bubble_sort(int *a, int n) {
    bool swapped;
    for (int i = 0; i < n - 1; i++) {
        swapped = false;
        for (int j = 0; j < n - i - 1; j++) {
            if (a[j] > a[j + 1]) {
                swap(&a[j], &a[j + 1]);
                swapped = true;
            }
        }
        if (swapped == false) {
            break;
        }
    }
}

void insertion_sort(int *a, int n) {
    int i, key, j;
    for (i = 1; i < n; i++) {
        key = a[i];
        j = i - 1;
        while (j >= 0 && a[j] > key) {
            a[j + 1] = a[j];
            j = j - 1;
        }
        a[j + 1] = key;
    }
}

static int partition(int *a, int n) {
    int pivot = a[0];
    
    int lower = 1;
    int upper = n - 1;

    do {
        while (a[lower] <= pivot && lower <= upper)
            lower++;
        while (a[upper] > pivot && lower <= upper)
            upper--;

        if (lower <= upper) {
            swap(&a[upper], &a[lower]);
            upper--;
            lower++;
        }
    } while (lower <= upper);
    swap(&a[upper], &a[0]);

    return upper;
}
void quick_sort(int *a, int n) {
    if (n <= 1){
        return;
    }
    int b = partition(a, n);
    quick_sort(a, b);
    quick_sort(a+b+1, n-b-1);
}

bool linear_search(const int *a, int n, int v) {
    for (int i = 0; i < n; i++) {
        if (a[i] == v) {
            return true;
        }
    }
    return false;
}

bool binary_search(const int *a, int n, int v) {
    int first = 0;
    int last = n - 1;
    int middle;

    while (first <= last) {
        middle = (first + last) / 2;

        if (a[middle] < v) {
            first = middle + 1;
        } else if (a[middle] > v) {
            last = middle - 1;
        } else {
            return true;
        }
    }
    return false;
}
