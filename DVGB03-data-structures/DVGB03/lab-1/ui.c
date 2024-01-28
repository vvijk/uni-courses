#include "ui.h"
#include "io.h"
#include "analyze.h"

#include <stdbool.h>
#include <stdio.h>
#include <math.h>

//
// Private
//

static void ui_invalid_input(){
	printf("info> bad input\n");
}

static void ui_exit(){
	printf("info> bye\n");
}

static char ui_get_choice(){
	char buf[3];

	printf("input> ");
	return read_line(buf, 3) ? buf[0] : 0;
}

static void ui_line(char c, int n){
	while (n-- > 0) {
		putchar(c);
	}
	putchar('\n');
}

static void ui_menu_options(const char *options[], int num_options){
	int i;

	for (i=0; i<num_options; i++) {
		printf("    %c) %s\n", 'a'+i, options[i]);
	}
}

static void ui_menu(){
	const char *options[] = {
		"Menu",
		"Exit\n",
		"Bubble sort best case",
		"Bubble sort worst case",
		"Bubble sort avarage case\n",
		"Insertion sort best case",
		"Insertion sort worst case",
		"Insertion sort avarage case\n",
		"Quicksort best case",
		"Quicksort worst case",
		"Quicksort avarage case\n",
		"Linear search sort best case",
		"Linear search sort worst case",
		"Linear search sort avarage case\n",
		"Binary search sort best case",
		"Binary search sort worst case",
		"Binary search sort avarage case",
	};

	ui_line('=', MENU_WIDTH);
	ui_menu_options(options, sizeof(options) / sizeof(char *));
	ui_line('-', MENU_WIDTH);
}

//-------------
static const char* complex_str[] = {
	[0] = "Size\tTime T(s)\tT/logn\t\tT/n\t\tT/nlogn\n",
	[1] = "Size\tTime T(s)\tT/nlogn\t\tT/n^2\t\tT/n^3\n",
	[2] = "Size\tTime T(s)\tT/n\t\tT/nlogn\t\tT/n^2\n",
	[3] = "Size\tTime T(s)\tT/logn\t\tT/1\t\tT/n\n",
	[4] = "Size\tTime T(s)\tT/1\t\tT/logn\t\tT/n\n",
};
void ui_resultlist(algorithm_t a, case_t c, result_t *res, int n, char *header){
	ui_line('*', MENU_WIDTH*2);
	printf("\t\t\t\t%s\n", header);
	ui_line('~', MENU_WIDTH*2);

	for(int i = 0; i < n; i++){
		long int n =  res[i].size;
		double time = res[i].time;
		switch(a){
		case bubble_sort_t:
		if(c == best_t){ //O(n)
			if(i==0){printf("%s", complex_str[0]);};
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time, time/log(n), (time/n), time/(n*log(n)));
		}else if(c == worst_t){//O(n^2)
			if(i==0){printf("%s", complex_str[1]);};
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time,  time/(n*log(n)), time/(n*n), (time/(n*n*n)));
		}else{//O(n^2)
			if(i==0){printf("%s", complex_str[1]);};
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time,  time/(n*log(n)), time/(n*n), (time/(n*n*n)));
		}
		break;
		case insertion_sort_t:
		if(c==best_t){
			if(i==0) printf("%s", complex_str[0]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time, time/log(n), (time/n), time/(n*log(n)));
		}else if(c==worst_t){
			if(i==0) printf("%s", complex_str[1]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time,  time/(n*log(n)), time/(n*n), (time/(n*n*n)));
		}else{
			if(i==0) printf("%s", complex_str[1]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time,  time/(n*log(n)), time/(n*n), (time/(n*n*n)));
		}
		break;
		case quick_sort_t:
			if(c==best_t){
			if(i==0) printf("%s", complex_str[2]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time, time/n, time/(n*log(n)), (time/(n*n)));
		}else if(c==worst_t){
			if(i==0) printf("%s", complex_str[1]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time,  time/(n*log(n)), time/(n*n), (time/(n*n*n)));
		}else{
			if(i==0) printf("%s", complex_str[2]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time, time/n, time/(n*log(n)), (time/(n*n)));
		}
		break;
		case binary_search_t:
		if(c==best_t){
			if(i==0) printf("%s", complex_str[3]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time, time/log(n), time/1, time/n);
		}else if(c==worst_t){
			if(i==0) printf("%s", complex_str[4]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time,  time/1, time/log(n), time/n);
		}else{
			if(i==0) printf("%s", complex_str[4]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time,  time/1, time/log(n), time/n);
		}
		break;
		case linear_search_t:
		if(c==best_t){
			if(i==0) printf("%s", complex_str[3]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time, time/log(n), time/1, time/n);
		}else if(c==worst_t){
			if(i==0) printf("%s", complex_str[0]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time,  time/log(n), time/n, time/(n*log(n)));
		}else{
			if(i==0) printf("%s", complex_str[0]);
			printf("%ld\t%.10f\t%e\t%e\t%e\n", n, time,  time/log(n), time/n, time/(n*log(n)));
		}
		break;
		}
	}
	ui_line('~', MENU_WIDTH*2);
}

//
// Public
//
void ui_run(){
	bool running, show_menu;
	result_t result[RESULT_ROWS];
	
	show_menu = true;
	running = true;
	while (running) {
		if (show_menu) {
			show_menu = false;
			ui_menu();
		}
		switch (ui_get_choice()) {
			// House keeping
			case 'a':
				show_menu = true;
				break;
			case 'b':
				running = false;
				break;
			case 'c':
				benchmark(bubble_sort_t, best_t, result, RESULT_ROWS);
				ui_resultlist(bubble_sort_t, best_t, result, RESULT_ROWS, "bubble sort:best");
				break;
			case 'd':
				benchmark(bubble_sort_t, worst_t, result, RESULT_ROWS);
				ui_resultlist(bubble_sort_t, worst_t, result, RESULT_ROWS, "bubble sort:worst");
				break;
			case 'e':
				benchmark(bubble_sort_t, average_t, result, RESULT_ROWS);
				ui_resultlist(bubble_sort_t, average_t, result, RESULT_ROWS, "bubble sort:average");
				break;
			case 'f':
				benchmark(insertion_sort_t, best_t, result, RESULT_ROWS);
				ui_resultlist(insertion_sort_t, best_t, result, RESULT_ROWS, "Insertion sort:best");
				break;
			case 'g':
				benchmark(insertion_sort_t, worst_t, result, RESULT_ROWS);
				ui_resultlist(insertion_sort_t, worst_t, result, RESULT_ROWS, "Insertion sort:worst");
				break;
			case 'h':
				benchmark(insertion_sort_t, average_t, result, RESULT_ROWS);
				ui_resultlist(insertion_sort_t, average_t, result, RESULT_ROWS, "Insertion sort:average");
				break;
			case 'i':
				benchmark(quick_sort_t, best_t, result, RESULT_ROWS);
				ui_resultlist(quick_sort_t, best_t, result, RESULT_ROWS, "Quicksort:best");
				break;
			case 'j':
				benchmark(quick_sort_t, worst_t, result, RESULT_ROWS);
				ui_resultlist(quick_sort_t, worst_t, result, RESULT_ROWS, "Quicksort:worst");
				break;
			case 'k':
				benchmark(quick_sort_t, average_t, result, RESULT_ROWS);
				ui_resultlist(quick_sort_t, average_t, result, RESULT_ROWS, "Quicksort:average");
				break;
			case 'l':
				benchmark(linear_search_t, best_t, result, RESULT_ROWS);
				ui_resultlist(linear_search_t, best_t, result, RESULT_ROWS, "Linear search:best");
				break;
			case 'm':
				benchmark(linear_search_t, worst_t, result, RESULT_ROWS);
				ui_resultlist(linear_search_t, worst_t, result, RESULT_ROWS, "Linear search:worst");
				break;
			case 'n':
				benchmark(linear_search_t, average_t, result, RESULT_ROWS);
				ui_resultlist(linear_search_t, average_t, result, RESULT_ROWS, "Linear search:average");
				break;
			case 'o':
				benchmark(binary_search_t, best_t, result, RESULT_ROWS);
				ui_resultlist(binary_search_t, best_t, result, RESULT_ROWS, "Binary search:best");
				break;
			case 'p':
				benchmark(binary_search_t, worst_t, result, RESULT_ROWS);
				ui_resultlist(binary_search_t, worst_t, result, RESULT_ROWS, "Binary search:worst");
				break;
			case 'q':
				benchmark(binary_search_t, average_t, result, RESULT_ROWS);
				ui_resultlist(binary_search_t, average_t, result, RESULT_ROWS, "Binary search:average");
				break;
			// Invalid input
			default:
				show_menu = false;
				ui_invalid_input();
				break;
		}
	}
	ui_exit();
}
