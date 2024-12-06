CLI = dotnet run --framework net7.0

CONTROLLER ?= 405419896
DOOR ?= 3
CARD ?= 10058400
MISSING_CARD ?= 10058399
ADDRESS ?= 192.168.1.100
NETMASK ?= 255.255.255.0
GATEWAY ?= 192.168.1.1
LISTENER ?= 192.168.1.100:60001
INTERVAL ?= 30
EVENT_INDEX ?= 23
RECORD_SPECIAL_EVENTS ?= true
TIME_PROFILE ?= 29
TASK ?= 4
PCCONTROL ?= true
START_DATE ?= 2024-02-03
END_DATE ?= 2024-12-29
PERMISSIONS ?= "1,2,4:17"
PIN ?= 7531
INTERLOCK ?= "1&2,3&4"

clean:
	dotnet clean

format:
	dotnet format

build: format
	dotnet build

run: build
	$(CLI)

find-controllers: build
	$(CLI) find-controllers

get-controller: build
	$(CLI) get-controller --controller $(CONTROLLER)

set-IPv4: build
	$(CLI) set-IPv4  --controller $(CONTROLLER) --address $(ADDRESS) --netmask 255.255.255.0 --gateway 192.168.1.1

get-listener: build
	$(CLI) get-listener --controller $(CONTROLLER)

set-listener: build
	$(CLI) set-listener --controller $(CONTROLLER) --listener $(LISTENER) --interval $(INTERVAL)

get-time: build
	$(CLI) get-time --controller $(CONTROLLER)

set-time: build
	$(CLI) set-time --controller $(CONTROLLER)

get-door: build
	$(CLI) get-door --controller $(CONTROLLER) --door $(DOOR)

set-door: build
	$(CLI) set-door --controller $(CONTROLLER) --door $(DOOR) --mode controlled --delay 7  

set-door-passcodes: build
	$(CLI) set-door-passcodes --controller $(CONTROLLER) --door $(DOOR) --passcodes 12345,54321,999999

open-door: build
	$(CLI) open-door --controller $(CONTROLLER)

get-status:
	$(CLI) get-status --controller $(CONTROLLER)

get-cards: build
	$(CLI) get-cards --controller $(CONTROLLER)

get-card: build
	$(CLI) get-card --controller $(CONTROLLER) --card $(CARD)
	$(CLI) get-card --controller $(CONTROLLER) --card $(MISSING_CARD)

get-card-at-index: build
	$(CLI) get-card-at-index --controller $(CONTROLLER)

put-card: build
	$(CLI) put-card --controller $(CONTROLLER) --card $(CARD) --start-date $(START_DATE) --end-date $(END_DATE) --permissions $(PERMISSIONS) --PIN $(PIN)

delete-card: build
	$(CLI) delete-card --controller $(CONTROLLER) --card $(CARD)

delete-all-cards: build
	$(CLI) delete-all-cards --controller $(CONTROLLER)

get-event: build
	$(CLI) get-event --controller $(CONTROLLER) --index $(EVENT_INDEX)

get-event-index: build
	$(CLI) get-event-index --controller $(CONTROLLER)

set-event-index: build
	$(CLI) set-event-index --controller $(CONTROLLER) --index $(EVENT_INDEX)

record-special-events:build
	$(CLI) record-special-events --controller $(CONTROLLER) --enable $(RECORD_SPECIAL_EVENTS)

get-time-profile:build
	$(CLI) get-time-profile --controller $(CONTROLLER) --profile $(TIME_PROFILE)

set-time-profile:build
	$(CLI) set-time-profile --controller $(CONTROLLER) --profile $(TIME_PROFILE) \
                                                       --start 2024-01-01 --end 2024-12-31 \
                                                       --weekdays Mon,Tues,Fri \
                                                       --segments 08:30-11:30,12:15-16:45,19:30-21:15 \
                                                       --linked 37

clear-time-profiles:build
	$(CLI) clear-time-profile --controller $(CONTROLLER)

add-task:build
	$(CLI) add-task --controller $(CONTROLLER) \
                    --task $(TASK) \
                    --door $(DOOR) \
                    --start-date 2024-01-01 --end-date 2024-12-31 --start-time 09:45 \
                    --weekdays Mon,Tue,Fri \
                    --more-cards 7

clear-tasklist: build
	$(CLI) clear-tasklist --controller $(CONTROLLER)

refresh-tasklist: build
	$(CLI) refresh-tasklist --controller $(CONTROLLER)

set-pc-control: build
	$(CLI) set-pc-control --controller $(CONTROLLER) --enable $(PCCONTROL)

set-interlock: build
	$(CLI) set-interlock --controller $(CONTROLLER) --interlock $(INTERLOCK)

