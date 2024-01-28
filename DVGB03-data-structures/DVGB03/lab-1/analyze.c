#include "analyze.h"
#include "algorithm.h"

#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#define TIMES 100
#define BILLION 1E9
#define HUNDRED 100000
//
// Private
//

//
// Public
//
void fillArray(int n, int arr[], case_t c) {
    int temp = n;
    if (c == best_t) {
        for (int i = 0; i < n; i++) {
            arr[i] = i;
        }
    } else if (c == average_t) {
        for (int i = 0; i < n; i++) {
            arr[i] = rand() % 100;
        }
    } else {
        for (int i = 0; i < n; i++) {
            arr[i] = temp;
            temp--;
        }
    }
}
void setPivot(int *a, int piv) {
    a[0] = piv;
}
// clock_t start_t, end_t;
/*
start_t = clock();
bubble_sort(array, n);
end_t = clock();
double time_spent = (double)(start_t - end_t) / CLOCKS_PER_SEC;
return time_spent;
*/
double timeCalc(int *array, algorithm_t a, int n, case_t c) {
    struct timespec start, stop;
    double total = 0;
    switch (a) {
        case bubble_sort_t:
            clock_gettime(CLOCK_MONOTONIC, &start);
            bubble_sort(array, n);
        break;

        case insertion_sort_t:
            clock_gettime(CLOCK_MONOTONIC, &start);
            insertion_sort(array, n);
        break;

        case quick_sort_t:
            switch (c) {
            case best_t:
                // sortera random?
                fillArray(n, array, average_t);
                setPivot(array, array[n / 2 - 1]);
                clock_gettime(CLOCK_MONOTONIC, &start);
                quick_sort(array, n);
                break;
            case worst_t:
                fillArray(n, array, best_t);
                setPivot(array, 1);
                clock_gettime(CLOCK_MONOTONIC, &start);
                quick_sort(array, n);
                break;
            case average_t:
                setPivot(array, rand() % 100);
                clock_gettime(CLOCK_MONOTONIC, &start);
                quick_sort(array, n);
                break;
            }
        break;

        case linear_search_t:
            switch (c) {
            case best_t:
                clock_gettime(CLOCK_MONOTONIC, &start);
                for (int i = 0; i < HUNDRED; i++) {
                    linear_search(array, n, 1);
                }
                break;
            case worst_t:
                clock_gettime(CLOCK_MONOTONIC, &start);
                for (int i = 0; i < HUNDRED; i++) {
                    linear_search(array, n, n - 1);
                }
                break;
            case average_t:
                clock_gettime(CLOCK_MONOTONIC, &start);
                for (int i = 0; i < HUNDRED; i++) {
                    linear_search(array, n, n / 2 - 1);
                }
                break;
            }
        break;

        case binary_search_t:
            switch (c) {
            case best_t:
                clock_gettime(CLOCK_MONOTONIC, &start);
                for (int i = 0; i < HUNDRED; i++) {
                    binary_search(array, n, array[n / 2 - 1]);
                }
                break;
            case worst_t:
                clock_gettime(CLOCK_MONOTONIC, &start);
                for (int i = 0; i < HUNDRED; i++) {
                    binary_search(array, n, array[n - 1]);
                }
                break;
            case average_t:
                clock_gettime(CLOCK_MONOTONIC, &start);
                for (int i = 0; i < HUNDRED; i++) {
                    binary_search(array, n, 1);
                }
                break;
            }
        break;
    }
    clock_gettime(CLOCK_MONOTONIC, &stop);
    total = (stop.tv_sec * BILLION + stop.tv_nsec) - (start.tv_sec * BILLION + start.tv_nsec);

    if (a == binary_search_t || a == linear_search_t) {
        total = total / HUNDRED;
    }
    return total;
}

void benchmark(const algorithm_t a, const case_t c, result_t *buf, int n) {
    for (int i = 0; i < n; i++) {
        int arraySize = 0;
        double time = 0;

        for (int j = 0; j < ITERATIONS; j++) {
            arraySize = SIZE_START * pow(2, i);
            int array[arraySize];
            buf[i].size = arraySize;

            if (a != binary_search_t && a != linear_search_t) {
                fillArray(arraySize, array, c);
            } else {
                fillArray(arraySize, array, best_t);
            }

            switch (a) {
            case bubble_sort_t:
                time += timeCalc(array, bubble_sort_t, arraySize, c);
                break;
            case insertion_sort_t:
                time += timeCalc(array, insertion_sort_t, arraySize, c);
                break;
            case quick_sort_t:
                time += timeCalc(array, quick_sort_t, arraySize, c);
                break;
            case linear_search_t:
                time += timeCalc(array, linear_search_t, arraySize, c);
                break;
            case binary_search_t:
                time += timeCalc(array, binary_search_t, arraySize, c);
                break;
            }
        }
        time = (time / ITERATIONS) / BILLION;
        buf[i].time = time;
        /*
        buf[i].slower = time/log(n);
        buf[i].middle = time/n;
        buf[i].faster = time/(n*log(n));
        */
    }
}
/*
Problem:
    Binary search best n/2 (-1)  !!

*/